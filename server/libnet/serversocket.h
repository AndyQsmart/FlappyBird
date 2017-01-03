#ifndef SERVERSOCKET_H
#define SERVERSOCKET_H

class Socket;

class ServerSocket
{
private:
	int sock_fd;
	static const int BACKLOG = 10;

public:
	ServerSocket(int port);
	Socket *acceptClient();
	int getFileDescriptor();
	void setNonBlocking();
};

#endif // SERVERSOCKET_H
