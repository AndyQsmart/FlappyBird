#include <iostream>
#include <cstdio>
#include <cstring>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>
#include "serversocket.h"
#include "socket.h"
#include "filetool.h"
#include "socketexception.h"

int main()
{
	char client_ip[16];
	char buffer[1024];
	try
	{
		ServerSocket *server_socket = new ServerSocket(8888);
		Socket *socket = NULL;
		for (;;)
		{
			try
			{
				socket = server_socket->acceptClient();
				printf("INFO: connect to client\n");
				socket->receive(buffer, sizeof(buffer)-1);
				printf("INFO:Connect to: %s\n", buffer);
				FileTool *filetool = new FileTool(buffer);
				for (;;)
				{
					socket->receive(buffer, sizeof(buffer)-1);
					printf("INFO: get score: %s\n", buffer);
					int bestscore;
					sscanf(buffer, "%d", &bestscore);
					bestscore = filetool->setBestScore(bestscore);
					printf("DEBUG: bestscore: %d\n", bestscore);
					sprintf(buffer, "%d", bestscore);
					printf("DEBUG: buffer len: %d\n", strlen(buffer));
					socket->send(buffer, strlen(buffer));
				}
				//socket->closeSocket();
			}
			catch (SocketException e)
			{
				e.print();
			}
		}
		socket->closeSocket();
	}
	catch (SocketException e)
	{
		e.print();
	}
/*
		inet_ntop(AF_INET, &(client_addr.sin_addr), client_ip, sizeof(client_ip));
		printf("INFO:accept client:%s\n", client_ip);
*/
	return 0;
}
