LIBDATA = filetool.o

LIBNET = serversocket.o \
	socket.o \
	socketexception.o	

LIBMESSAGE = messagebuffer.o \
	messagetool.o

OBJECTS = $(LIBDATA) \
	$(LIBNET) \
	$(LIBMESSAGE) \
	epolltool.o \
	main.o

server: $(OBJECTS)
	g++ -o server $(OBJECTS)

main.o: main.cpp
	g++ -c main.cpp -std=c++11

$(LIBDATA): libdata/filetool.cpp libdata/filetool.h
	g++ -c $<

$(LIBNET): libnet/serversocket.cpp \
		libnet/serversocket.h \
		libnet/socket.cpp \
		libnet/socket.h \
		libnet/socketexception.cpp \
		libnet/socketexception.h
	g++ -c $^

$(LIBMESSAGE): libmessage/messagebuffer.cpp \
			libmessage/messagebuffer.h \
			libmessage/messagetool.cpp \
			libmessage/messagetool.h
	g++ -c $^

epolltool.o:epolltool.cpp epolltool.h
	g++ -o $@ -c $<

clean:
	rm -f $(OBJECTS) server
