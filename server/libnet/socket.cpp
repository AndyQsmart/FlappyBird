#include <iostream>
#include <cstdio>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>
#include <fcntl.h>
#include "socket.h"
#include "socketexception.h"

#define LOG_DEBUG std::cout << __FILE__ << ":" << __LINE__ << "{" << __FUNCTION__ << "}DEBUG|"
#define LOG_ERROR std::cout << __FILE__ << ":" << __LINE__ << "{" << __FUNCTION__ << "}DEBUG|"

Socket::Socket(int new_sock_fd)
{
	sock_fd = new_sock_fd;
}

Socket::Socket(char *server_ip, int port)
{
	int a1, a2, a3, a4;
	if (server_ip == NULL ||
		sscanf(server_ip, "%d.%d.%d.%d", &a1, &a2, &a3, &a4) != 4)
	{
		LOG_ERROR << "socket error!" << std::endl;
		throw SocketException("socket error!");
	}
	sock_fd = socket(AF_INET, SOCK_STREAM, 0);
	if (sock_fd == -1)
	{
		LOG_ERROR << "socket error!" << std::endl;
		throw SocketException("socket error!");
	}
	struct sockaddr_in server_addr;
	server_addr.sin_family = AF_INET;
	server_addr.sin_port = htons(port);
	server_addr.sin_addr.s_addr = htonl((a1<<24)|(a2<<16)|(a3<<8)|a4);
	if (connect(sock_fd, (struct sockaddr *)&server_addr, sizeof(server_addr)) == -1)
	{
		LOG_ERROR << "connect to server error" << std::endl;
		throw SocketException("connect to server error");
	}
}

int Socket::receive(char *buffer, int len)
{
	
	ssize_t buffer_len = read(sock_fd, buffer, len);
	if (buffer_len < 0)
	{
		LOG_ERROR << "read error" << std::endl;
		throw SocketException("read error");
	}
	return buffer_len;
}

void Socket::send(char *buffer, int len)
{
	write(sock_fd, buffer, len);
}

void Socket::closeSocket()
{
	close(sock_fd);
}

int Socket::getFileDescriptor()
{
	return sock_fd;
}

void Socket::setNonBlocking()
{
	int opts = fcntl(sock_fd, F_GETFL);
	if (opts < 0)
	{
		LOG_ERROR << "fcntl error" << std::endl;
		throw SocketException("fcntl error");
	}
	opts = opts|O_NONBLOCK;
	if (fcntl(sock_fd, F_SETFL, opts) < 0)
	{
		LOG_ERROR << "fcntl error" << std::endl;
		throw SocketException("fcntl error");
	}
}
