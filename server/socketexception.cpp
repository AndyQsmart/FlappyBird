#include <iostream>
#include "socketexception.h"

void SocketException::print()
{
	if (data == NULL) std::cout << "NULL" << std::endl;
	else std::cout << data << std::endl;
}
