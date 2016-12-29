OBJECTS = main.o \
	filetool.o \
	serversocket.o \
	socket.o \
	socketexception.o \

server: $(OBJECTS)
	g++ -o server $(OBJECTS)

main.o: main.cpp
	g++ -c main.cpp

filetool.o: filetool.cpp filetool.h
	g++ -c filetool.cpp

serversocket.o: serversocket.cpp serversocket.h
	g++ -c serversocket.cpp

socket.o: socket.cpp socket.h
	g++ -c socket.cpp

socketexception.o: socketexception.cpp socketexception.h
	g++ -c socketexception.cpp

clean:
	rm -f $(OBJECTS) server
