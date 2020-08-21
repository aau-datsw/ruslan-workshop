#ifndef HTTP_H
#define HTTP_H

#define POST 1
#define GET 2

#ifdef __cplusplus
#define RESTRICT
#else
#define RESTRICT restrict
#endif

typedef unsigned short req_t;

#ifdef __cplusplus
extern "C"
{
#endif

struct property
{
    char *name, *value;
};

struct http
{
    unsigned property_count;
    struct property *header_properties;
    req_t request_type;
    char *page;
};

#ifdef __cplusplus
}
#endif

// Prototypes.
struct http http_init(const char *page, req_t request_type);
const char *http_str(struct http request, const char *post_args);
void http_add_header_property(struct http *RESTRICT request, const char *name, const char *value);

#endif