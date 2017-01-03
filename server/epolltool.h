#ifndef EPOLLTOOL_H
#define EPOLLTOOL_H

struct epoll_event;

class EpollTool
{
private:
	const static int time_out_ms = -1;
	int epoll_fd;

public:
	EpollTool(int size);
	int waitEvents(epoll_event *events, int maxlen);
	void registerEvent(int socket_fd);
};

#endif // EPOLLTOLL_H
