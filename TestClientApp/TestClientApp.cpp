#include <iostream>
#include "boost/asio.hpp"
#include "boost/asio/co_spawn.hpp"
#include <stdlib.h>
#include <thread>

using namespace boost::asio;

ip::address getBroadcastSubnet() {
    return ip::address::from_string("200.0.0.255");
}

int main()
{
    std::cout << "Type 's' and press enter to start sending...";
    char selection = 'c';
    while (selection != 's') {
        std::cin >> selection;
    }

    std::cout << "Starting sending!\n";
    io_context context;
     
    ip::udp::endpoint remote(getBroadcastSubnet(), 25520);
    ip::udp::socket socket(context, remote);

    std::cout << "End sending";
    return 1;
}


