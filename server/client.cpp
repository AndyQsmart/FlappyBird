#include <iostream>
#include <cstdio>
#include <cstdlib>
#include <cstring>
#include <string>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>
using namespace std;

int connectServer(const char *server_ip, int port)
{
	int a1, a2, a3, a4;
	if (server_ip == NULL ||
		sscanf(server_ip, "%d.%d.%d.%d", &a1, &a2, &a3, &a4) != 4)
	{
		perror("socket error!");
		return -1;
	}
	int sock_fd = socket(AF_INET, SOCK_STREAM, 0);
	if (sock_fd == -1)
	{
		perror("socket error!");
		return -1;
	}
	struct sockaddr_in server_addr;
	server_addr.sin_family = AF_INET;
	server_addr.sin_port = htons(port);
	server_addr.sin_addr.s_addr = htonl((a1<<24)|(a2<<16)|(a3<<8)|a4);
	if (connect(sock_fd, (struct sockaddr *)&server_addr, sizeof(server_addr)) == -1)
	{
		perror("connect to server error");
		return -1;
	}
	return sock_fd;
}

int main(int argc, char **argv)
{
	int sock_fd = connectServer(argv[1], 8888);
	if (sock_fd == -1) return 0;
	char buffer[40960];
	string msg;
	for (;;)
	{
		cout << "please enter message:";
		getline(cin, msg);
		if (!msg.empty())
		{
			int *ptr = (int *)buffer;
			*ptr = msg.size();			
			strncpy(buffer+4, msg.c_str(), msg.size());
			write(sock_fd, buffer, 4+msg.size());
			cout << "send successfully" << endl;
			ssize_t buffer_len = read(sock_fd, buffer, sizeof(buffer)-1);
			if (buffer_len == -1)
			{
				perror("read from server error!");
				close (sock_fd);
				return 0;
			}
			else if (buffer_len == 0)
			{
				perror("read no message from server");
			}
			else
			{
				buffer[buffer_len] = '\0';
				printf("read message from server:%s\n", buffer+4);
			}
			buffer_len = read(sock_fd, buffer, sizeof(buffer)-1);
			if (buffer_len == -1)
			{
				perror("read from server error!");
				close (sock_fd);
				return 0;
			}
			else if (buffer_len == 0)
			{
				perror("read no message from server");
			}
			else
			{
				buffer[buffer_len] = '\0';
				printf("read message from server:%s\n", buffer+4);
			}

		}
	}
	close(sock_fd);
	return 0;
}
