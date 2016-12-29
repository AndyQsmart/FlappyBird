#include <cstdio>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>
#include "socket.h"
#include "socketexception.h"

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
		throw SocketException("socket error!");
	}
	sock_fd = socket(AF_INET, SOCK_STREAM, 0);
	if (sock_fd == -1)
	{
		throw SocketException("socket error!");
	}
	struct sockaddr_in server_addr;
	server_addr.sin_family = AF_INET;
	server_addr.sin_port = htons(port);
	server_addr.sin_addr.s_addr = htonl((a1<<24)|(a2<<16)|(a3<<8)|a4);
	if (connect(sock_fd, (struct sockaddr *)&server_addr, sizeof(server_addr)) == -1)
	{
		throw SocketException("connect to server error");
	}
}

void Socket::receive(char *buffer, int len)
{
	
	ssize_t buffer_len = read(sock_fd, buffer, len);
	if (buffer_len == -1)
	{
		throw SocketException("read error");
	}
	buffer[buffer_len] = '\0';
}

void Socket::send(char *buffer, int len)
{
	write(sock_fd, buffer, len);
}

void Socket::closeSocket()
{
	close(sock_fd);
}
