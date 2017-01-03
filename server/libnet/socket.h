#ifndef SOCKET_H
#define SOCKET_H

class Socket
{
private:
	int sock_fd;

public:
	Socket(int new_sock_fd);
	Socket(char *server_ip, int port);
	int receive(char *buffer, int len);
	void send(char *buffer, int len);
	void closeSocket();
	int getFileDescriptor();
	void setNonBlocking();
};

#endif // SOCKET_H
