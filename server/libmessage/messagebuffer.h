#ifndef MESSAGEBUFFER_H
#define MESSAGEBUFFER_H

/*
 *four bytes for every head
 *if enough for a package, then
 *return a messagetool
 *the MAXSIZE, must enough for a package at least
 */

class MessageBuffer
{
private:
	static const int MAXSIZE = 10240;
	char buffer[MAXSIZE];
	int from, to;

public:
	MessageBuffer();
	void addBuffer(char *new_buffer, int buffer_len);
	bool getPackage(char *package, int &buffer_len);
	bool isAlmostFull();
};

#endif // MESSAGEBUFFER_H
