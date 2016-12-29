#include <cstring>
#include <sys/socket.h>
#include <netinet/in.h>
#include "serversocket.h"
#include "socketexception.h"
#include "socket.h"

ServerSocket::ServerSocket(int port)
{
	sock_fd = socket(AF_INET, SOCK_STREAM, 0);
	if (sock_fd == -1)
	{
		//perror("socket error");
		throw SocketException("socket error");
	}

	//
	int flag = 1;
	setsockopt(flag, SOL_SOCKET, SO_REUSEADDR, (const void *)&flag, sizeof(flag));

	struct sockaddr_in my_addr;
	memset(&my_addr, 0, sizeof(my_addr));
	my_addr.sin_family = AF_INET;
	my_addr.sin_port = htons(port);
	my_addr.sin_addr.s_addr = htonl(INADDR_ANY);

	if (bind(sock_fd, (struct sockaddr *)&my_addr, sizeof(my_addr)) == -1)
	{
		//perror("bind address error!");
		throw SocketException("bind address error!");
	}

	if (listen(sock_fd, BACKLOG) == -1)
	{
		//perror("listen error!");
		throw SocketException("listen error!");
	}
}

Socket *ServerSocket::acceptClient()
{
	struct sockaddr_in client_addr;
	memset(&client_addr, 0, sizeof(client_addr));
	socklen_t client_addr_len = sizeof(client_addr);
	int client_fd = accept(sock_fd, (struct sockaddr *)&client_addr, &client_addr_len);
	if (client_fd == -1)
	{
		throw SocketException("accept client error!");
	}
	Socket *socket = new Socket(client_fd);
	return socket;
}
