#ifndef SOCKET_H
#define SOCKET_H

class Socket
{
private:
	int sock_fd;

public:
	Socket(int new_sock_fd);
	Socket(char *server_ip, int port);
	void receive(char *buffer, int len);
	void send(char *buffer, int len);
	void closeSocket();
};

#endif // SOCKET_H