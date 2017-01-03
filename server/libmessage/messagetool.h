#ifndef MESSAGETOOL_H
#define MESSAGETOOL_H

#include <string>

/*
 *4 byte: the length of the package
 *4 byte: the type of the package
 *4 byte: the length of string
 *
 *if position: 4 byte + 4 byte: x and y of bird
 *if socre: 4 byte: score of client
 */

class MessageTool
{
public:
	enum MessageType
	{
		CS_LOGIN = 0,
		CS_POSITION = 1,
		CS_OVER = 2,
		SC_LOGIN = 3,
		SC_POSITION = 4,
		SC_OVER = 5
	};

private:
	template<typename T>
	void getValue(char *buffer, T &val);
	template<typename T>
	void addValue(char *buffer, T val);
	void createHead(char *buffer, int buffer_len);
	void addHead(char *buffer, int &buffer_len);

public:
	MessageTool();
	int getMessageType(char *buffer, int buffer_len);
	void createLogMessage(char *buffer, int &buffer_len, char *username, int username_len);
	void getLogMessage(char *buffer, int buffer_len, char *username, int &username_len);
	void createFlyMessage(char *buffer, int &buffer_len, char *username, int username_len, float x, float y);
	void getFlyMessage(char *buffer, int buffer_len, char *username, int &username_len, float &x, float &y);
	void createOverMessage(char *buffer, int &buffer_len, char *username, int username_len, int score);
	void getOverMessage(char *buffer, int buffer_len, char *username, int &username_len, int &score);
};

template<typename T>
void MessageTool::getValue(char *buffer, T &val)
{
	int unit = sizeof(T);
	val = 0;
	char *ptr = (char *)&val;
	for (int i = 0; i < unit; ++i)
	ptr[i] = buffer[i];
}

template<typename T>
void MessageTool::addValue(char *buffer, T val)
{
	T *ptr = (T *)buffer;
	*ptr = val;
}

#endif // MESSAGETOOL_H
