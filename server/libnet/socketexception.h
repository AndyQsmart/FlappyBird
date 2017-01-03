#ifndef SOCKETEXCEPTION_H
#define SOCKETEXCEPTION_H

#include <cstdio>

class SocketException
{
private:
	const char *data;
public:
	SocketException(const char *msg = NULL);

	void print();
};

#endif // SOCKETEXCEPTION_H
