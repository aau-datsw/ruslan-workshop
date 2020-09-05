CC=gcc
INC:=../lib/json-parser ../framework
LINK:=../framework/transmitter.c ../framework/http.c ../lib/json-parser/json.c
MACRODEFS=-DLINUX
CFLAGS=$(foreach l,$(LINK),$(l)) -lm -pthread $(foreach lib,$(INC),-I $(lib)) $(MACRODEFS)
WINFLAGS=-lws2_32

program: program.c
ifeq ($(PLATFORMFLAG), -DWINDOWS)
	$(CC) -o program $(SRC) $(CFLAGS) $(WINFLAGS)
else
	$(CC) -o program $(SRC) $(CFLAGS)
endif

clean:
	rm program
