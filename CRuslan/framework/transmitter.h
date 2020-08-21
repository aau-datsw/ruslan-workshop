#ifndef TRANSMITTER_H
#define TRANSMITTER_H

#include <time.h>

// Defines record of groups.
typedef struct
{
    char *name;
    unsigned balance, stock_count, stock_value, total_value;
} group;

int *get_market_data(struct tm from, struct tm to);
group get_info();
void buy();
void sell();
void set_token(const char *token);

#endif