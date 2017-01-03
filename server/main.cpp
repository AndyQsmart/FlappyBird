#include <iostream>
#include <cstdio>
#include <cstring>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include "libnet/serversocket.h"
#include "libnet/socket.h"
#include "libdata/filetool.h"
#include "libnet/socketexception.h"
#include "epolltool.h"
#include "sys/epoll.h"
#include "libmessage/messagebuffer.h"
#include <unordered_map>

std::unordered_map<int, Socket *> sockets;
std::unordered_map<int, MessageBuffer *> buffers;
char buffer[1030];
ServerSocket *server_socket = NULL;
Socket *client_socket = NULL;
EpollTool *epoll_tool = NULL;
epoll_event events[100];
MessageBuffer *message_buffer = NULL;

void init()
{
	try
	{
		server_socket = new ServerSocket(8888);
		//server_socket->setNonBlocking();
	}
	catch (SocketException e)
	{
		e.print();
	}
	epoll_tool = new EpollTool(1024);
	epoll_tool->registerEvent(server_socket->getFileDescriptor());	
}

void allSend(Socket *socket, char *package, int buffer_len)
{
	std::unordered_map<int, Socket *>::iterator it;	
	for (it = sockets.begin(); it != sockets.end(); ++it)
	{
		if (it->second != socket)
			it->second->send(package, buffer_len);
	}
}

void work()
{
	int events_len = epoll_tool->waitEvents(events, 100);
	//printf("DEBUG: events len: %d\n", events_len);
	for (int i = 0; i < events_len; ++i)
	{
		if (events[i].data.fd == server_socket->getFileDescriptor())
		{
			printf("DEBUG: create a new connection\n");
			// a new connection
			client_socket = server_socket->acceptClient();
			if (client_socket == NULL) continue;
			//client_socket->setNonBlocking();
			epoll_tool->registerEvent(client_socket->getFileDescriptor());
			printf("INFO: client connected\n");
			sockets[client_socket->getFileDescriptor()] = client_socket;
			buffers[client_socket->getFileDescriptor()] = new MessageBuffer();
			//printf("%d\n", socket->getFileDescriptor());
			client_socket = NULL;
		}
		else if (events[i].events & EPOLLIN)
		{
			// a message from client
			std::unordered_map<int, Socket *>::iterator it;
			it = sockets.find(events[i].data.fd);
			if (it == sockets.end()) continue;
			client_socket = it->second;
			//printf("%d\n", events[i].data.fd);
			int buffer_len = 0;
			try
			{
				buffer_len = client_socket->receive(buffer, 1024);
			}
			catch (SocketException e)
			{
				e.print();
			}
			//printf("DEBUG: receive a package, len: %d\n", buffer_len);
			if (buffer_len <= 0) 
			{
				//printf("DEBUG: try close socket\n");
				client_socket->closeSocket();
				sockets.erase(events[i].data.fd);
				buffers.erase(events[i].data.fd);
				printf("INFO: socket closed\n");
			}
			else
			{
				message_buffer = buffers[events[i].data.fd];
				message_buffer->addBuffer(buffer, buffer_len);
				while (message_buffer->getPackage(buffer,  buffer_len))
				{
					/*
 					 * 服务器暂时只做包的广播功能，
 					 * 拆包在客户端完成，
 					 * 服务器的拆包功能已经在messagetool.cpp中实现，
 					 * 和C#客户端中代码逻辑一致。
 					 *
 					 */
					allSend(client_socket, buffer, buffer_len);
				}
			}
		}
	} 
}

int main()
{
	init();
	for (;;)
	{
		work();
	}
	return 0;
}
