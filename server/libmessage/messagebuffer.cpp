#include <cstdio>
#include <arpa/inet.h>
#include "messagebuffer.h"

MessageBuffer::MessageBuffer()
{
	from = 0;
	to = 0;
}

void MessageBuffer::addBuffer(char *new_buffer, int buffer_len)
{
	if (buffer_len > 100)
	//printf("DEBUG: buffer len: %d\n", buffer_len);
	if ((to-from+MAXSIZE)%MAXSIZE+buffer_len >= MAXSIZE) return;
	for (int i = 0; i < buffer_len; ++i)
	{
		buffer[to] = new_buffer[i];
		to = (to+1)%MAXSIZE;	
	}
}

bool MessageBuffer::getPackage(char *package, int &buffer_len)
{	
	//printf("DEBUG: message buffer len: %d\n", (to-from+MAXSIZE)%MAXSIZE);	
	if ((to-from+MAXSIZE)%MAXSIZE < 4) return false;

	/*
 	 *need to deal,
 	 *if the package is invaild
 	 */

	//get buffer len
	int len = 0;
	char *ptr = (char *)&len;
	for (int i = 0; i < 4; ++i)
		ptr[i] = buffer[(from+i)%MAXSIZE];
	len = ntohl(len);

	if ((to-from+MAXSIZE)%MAXSIZE < 4+len) return false;

	//deal package??
	buffer_len = len+4;
	for (int i = 0; i < buffer_len; ++i)
		package[i] = buffer[(from+i)%MAXSIZE];
	from = (from+buffer_len)%MAXSIZE;
	//printf("DEBUG: message len: %d\n", buffer_len);
	return true;
}

bool MessageBuffer::isAlmostFull()
{
	if ((to-from+MAXSIZE)%MAXSIZE < MAXSIZE-1024) return false;
	else return true;
}
