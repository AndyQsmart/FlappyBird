#ifndef SOCKETEXCEPTION_H
#define SOCKETEXCEPTION_H

#include <cstdio>

class SocketException
{
private:
	const char *data;
public:
	SocketException(const char *msg = NULL) : data(msg) {}

	void print();
};

#endif // SOCKETEXCEPTION_H
