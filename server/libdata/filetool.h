#ifndef FILETOOL_H
#define FILETOOL_H

class FileTool
{
private:
	FILE *fp;
	char filename[100];

public:
	FileTool(char *username);
	bool tryOpen();
	int setBestScore(int score);
};

#endif // FILETOOL_H