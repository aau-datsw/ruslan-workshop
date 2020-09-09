#include "transmitter.h"
#include "http.h"
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <stdio.h>
#include <json.h>

#ifdef char
#define char char
#endif

#ifdef WINDOWS
#include <winsock2.h>
#include <pthread.h>

typedef SOCKET sock_t;
#pragma comment(lib, "ws2_32.lib")
#else
#include <sys/socket.h>
#include <arpa/inet.h>
#include <netdb.h>

typedef int sock_t;
#endif

#define HOST "market-place"
#define PORT 3000

#define SELL 1
#define BUY 2

#define TWOCIPHER_STR(num) ({ \
    char str_num[3]; \
    (num) >= 10 ? sprintf(str_num, "%d\0", (num)) : sprintf(str_num, "0%d\0", (num)); \
    str_num; \
})

#define ISO8601(tm) ({ \
    char str[20]; \
    sprintf(str, "%d-%s-%sT%s:%s:%s\0", (tm).tm_year, TWOCIPHER_STR((tm).tm_mon + 1), TWOCIPHER_STR((tm).tm_mday), TWOCIPHER_STR((tm).tm_hour), TWOCIPHER_STR((tm).tm_min), TWOCIPHER_STR((tm).tm_sec)); \
    str; \
})

static char *x_token = NULL;

// Prototypes.
int *json_iarray(const char *json);
static sock_t sockfd_setup(const char *url, unsigned port);
static inline char *host_ip(struct hostent host);
static void *execute_post(void *arg);
static inline void future(void *(*job)(void *arg), void *arg);

// Stores HTTP token.
void set_token(const char *token)
{
    x_token = realloc(x_token, strlen(token) + 1);
    strcpy(x_token, token);
}

// TODO: Set MARKET_DATA_SZ to size of received array.
// Returns market data.
int *get_market_data(struct tm from, struct tm to)
{
    char *host = malloc(100);
    struct http request = http_init("/ruslan", GET);
    sprintf(host, "%s:%d/api/v1/market?from=%s&to=%s\0", HOST, PORT, ISO8601(from), ISO8601(to));
    http_add_header_property(&request, "X-Token", x_token);

    char *http_get = http_str(request, NULL);
    sock_t sockfd = sockfd_setup(host, 80);
    send(sockfd, http_get, strlen(http_get), 0);
    free(http_get);

    char *buffer = malloc(500);
    short received;

#ifdef WINDOWS
    if ((received = recv(sockfd, buffer, 500, 0)) == SOCKET_ERROR)
        return NULL;
#else
    if ((received = read(sockfd, buffer, 500)) <= 0)
        return NULL;
#endif

    buffer[received] = '\0';
    close(sockfd);
    free(host);
    return json_iarray(buffer);
}

// Converts JSON string into int array.
int *json_iarray(const char *json)
{
    json_value *value = json_parse((json_char *) json, strlen(json));

    if (value == NULL)
        return NULL;

    // TODO: Parse JSON array.
    json_value_free(value);
    return NULL;
}

// Returns record of group.
struct group get_info()
{

}

// Buys stock.
void buy()
{
    short opt = BUY;
    future(execute_post, &opt);
}

// TODO: Add error handling if error is returned as JSON field.
// TODO: Let buy() and sell() return short, create function get_latest_error() to return error message.
// Sells stock.
void sell()
{
    short opt = SELL;
    future(execute_post, &opt);
}

// Sets up socket connection and return socker file descriptor.
static sock_t sockfd_setup(const char *url, unsigned port)
{
    sock_t sockfd;

#ifdef WINDOWS
    WSADATA wsa;

    if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
        return -1;

    if ((sockfd = socket(AF_INET, SOCK_STREM, 0)) == INVALID_SOCKET)
        return -1;
#else
    if ((sockfd = socket(AF_INET, SOCK_STREAM, 0)) < 0)
        return -1;
#endif
    struct sockaddr_in address;
    struct hostent *host = gethostbyname(url);

    if (host == NULL)
        return -1;

    address.sin_addr.s_addr = inet_addr(host_ip(*host));
    address.sin_family = AF_INET;
    address.sin_port = htons(port);

    if (connect(sockfd, (struct sockaddr *) &address, sizeof(address)) < 0)
    {
        close(sockfd);
        return -1;
    }

    return sockfd;
}

// Returns last ip of hostent address list.
static inline char *host_ip(struct hostent host)
{
    struct in_addr **addr_list = (struct in_addr **) host.h_addr_list;
    unsigned i;
    char *ip = malloc(100);

    for (i = 0; addr_list[i] != NULL; i++)
    {
        strcpy(ip, inet_ntoa(*addr_list[i]));
    }

    return ip;
}

// Executes sell and buy transmission.
static void *execute_post(void *arg)
{
    struct http request = http_init("/ruslan", POST);
    http_add_header_property(&request, "X-Token", x_token);

    char *http_post = http_str(request, NULL);
    char *host = malloc(50);

    if (*((short *) arg) == SELL)
        sprintf(host, "%s:%d/api/v1/sell\0", HOST, PORT);

    else if (*((short *) arg) == BUY)
        sprintf(host, "%s:%d/api/v1/buy\0", HOST, PORT);

    sock_t sockfd = sockfd_setup(host, 80);
    send(sockfd, http_post, strlen(http_post), 0);
    close(sockfd);
    free(http_post);
    free(host);

    return NULL;
}

// Executes procedure asynchronously.
// Does not execute asynchronously on Windows.
static inline void future(void *(*job)(void *arg), void *arg)
{
#if defined(LINUX) || defined(MAC)
    if (fork() == 0)
        job(arg);
#else
    pthread_t t;
    pthread_create(&t, NULL, job, arg);
#endif
}
