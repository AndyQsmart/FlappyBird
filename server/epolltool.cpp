#include <sys/epoll.h> 
#include "epolltool.h"

EpollTool::EpollTool(int size)
{
	epoll_fd = epoll_create(size);
}

int EpollTool::waitEvents(epoll_event *events, int maxlen)
{
	return epoll_wait(epoll_fd, events, maxlen, time_out_ms);
}

void EpollTool::registerEvent(int socket_fd)
{
	if (socket_fd < 0) return;
	struct epoll_event ev;
	ev.data.fd = socket_fd;
	ev.events = EPOLLIN;
	epoll_ctl(epoll_fd, EPOLL_CTL_ADD, socket_fd, &ev);
}
