#include <iostream>
#include "socketexception.h"

#define LOG_DEBUG std::cout << __FILE__ << ":" << __LINE__ << "{" << __FUNCTION__ << "}|DEBUG|"
#define LOG_ERROR std::cout << __FILE__ << ":" << __LINE__ << "{" << __FUNCTION__ << "}|ERROR|"

SocketException::SocketException(const char *msg) : data(msg)
{
}

void SocketException::print()
{
	if (data == NULL) LOG_ERROR << "NULL" << std::endl;
	else LOG_ERROR << data << std::endl;
}
