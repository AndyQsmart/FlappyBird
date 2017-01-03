#include "messagetool.h"
#include <cstdio>
#include <arpa/inet.h>

MessageTool::MessageTool()
{
/*
	if (buffer_len < 8) return;
	char *ptr = buffer;
	type = getInt(ptr);
	ptr += 4;
	//get username
	int username_len = getInt(ptr); 
	ptr += 4;
	username = getString(ptr, username_len);
	ptr += username_len;
*/
}

void MessageTool::createHead(char *buffer, int buffer_len)
{
	int len = htonl(buffer_len);
	addValue(buffer, len);
}

void MessageTool::addHead(char *buffer, int &buffer_len)
{
	int len = htonl(buffer_len);
	for (int i = buffer_len-1; i >= 0; --i)
		buffer[i+4] = buffer[i];
	addValue(buffer, len);
	buffer_len += 4;
}

int MessageTool::getMessageType(char *buffer, int buffer_len)
{
	int type = -1;
	getValue(buffer+4, type);
	return type;
}

void MessageTool::createLogMessage(char *buffer, int &buffer_len, char *username, int username_len)
{
	//add head
	int len = 4+4+username_len;
	createHead(buffer, len);

	//add type
	int type = htonl(SC_LOGIN);
	addValue(buffer+4, type);
	
	//add string length
	addValue(buffer+8, username_len);

	//add string
	for (int i = 0; i < username_len; ++i)
		buffer[i+12] = username[i];	
}

void MessageTool::getLogMessage(char *buffer, int buffer_len, char *username, int &username_len)
{
	getValue(buffer+8, username_len);
	username_len = ntohl(username_len);
	for (int i = 0; i < username_len; ++i)
		username[i] = buffer[i+12];	
}

void MessageTool::createFlyMessage(char *buffer, int &buffer_len, char *username, int username_len, float x, float y)
{
	//add head
	int len = 4+4+username_len+4+4;
	createHead(buffer, len);
	
	//add type
	int type = htonl(SC_POSITION);
	addValue(buffer+4, type);
	
	//add string length
	addValue(buffer+8, username_len);
	
	//add string
	for (int i = 0; i < username_len; ++i)
		buffer[i+12] = username[i];

	//add x
	addValue(buffer+12+username_len, x);

	//add y
	addValue(buffer+12+username_len+4, y);
}

void MessageTool::getFlyMessage(char *buffer, int buffer_len, char *username, int &username_len, float &x, float &y)
{
	getValue(buffer+8, username_len);
	username_len = ntohl(username_len);
	for (int i = 0; i < username_len; ++i)
		username[i] = buffer[i+12];
	getValue(buffer+12+username_len, x);
	getValue(buffer+12+username_len+4, y);
}

void MessageTool::createOverMessage(char *buffer, int &buffer_len, char *username, int username_len, int score)
{
	//add head
	int len = 4+4+username_len+4;
	createHead(buffer, len);
	
	//add type
	int type = htonl(SC_OVER);
	addValue(buffer+4, len);
	
	//add string length
	addValue(buffer+8, username_len);

	//add string
	for (int i = 0; i < username_len; ++i)
		buffer[i+12] = username[i];

	//add score
	addValue(buffer+12+username_len, score);
}

void MessageTool::getOverMessage(char *buffer, int buffer_len, char *username, int &username_len, int &score)
{
	getValue(buffer+8, username_len);
	username_len = ntohl(username_len);
	for (int i = 0; i < username_len; ++i)
		username[i] = buffer[i+12];
	getValue(buffer+12+username_len, score);
}

/*
int getInt(char *buffer)
{
	int val = buffer[0];
	val += buffer[1]<<8;
	val += buffer[2]<<16;
	val += buffer[3]<<24;
	return val;
}

template<class T>
void getInt2(char *byte, int index, T &val)
{
	int unit = sizeof(T);
	val = 0;
	char *buffer = byte;
	char *ptr = (char *)&val;
	buffer += index*unit;
	for (int i = 0; i < unit; ++i)
		ptr[i] = buffer[i];	
}

int main()
{
	char ch[10];
	ch[0] = 0x11;
	ch[1] = 0x22;
	ch[2] = 0x33;
	ch[3] = 0x44;
	printf("%d\n", getInt(ch));
	int ans = 0;
	getInt2(ch, 0, ans);
	printf("%d\n", ans);
	return 0;
}
*/
