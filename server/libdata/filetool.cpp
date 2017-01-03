#include <iostream>
#include <cstdio>
#include <cstdlib>
#include <cstring>
#include "filetool.h"

FileTool::FileTool(char *username)
{
	strcpy(filename, username);
}

bool FileTool::tryOpen()
{
	if ((fp = fopen(filename, "r+")) == NULL)
	{
		if ((fp = fopen(filename, "w+")) == NULL)
		{
			return false;
		}
		else
		{	
			fclose(fp);
			return true;
		}
	}
	else
	{
		fclose(fp);
		return true;
	}		
}

int FileTool::setBestScore(int score)
{
	if (!tryOpen()) return score;
	int bestscore = 0;
	fp = fopen(filename, "r+");
	fscanf(fp, "%d", &bestscore);
	fclose(fp);
	fp = fopen(filename, "w+");
	bestscore = bestscore>score ? bestscore : score;
	fprintf(fp, "%d", bestscore);
	fclose(fp);
	return bestscore;
}
