# Easy-ChatServer

Hi everyone,

this Rep is using C# for an Easy integration creating a asynchronous chat-server on your network using the TPL-Libary.

Initialize:
ServerAsync server = new ServerAsync();

Start listening:

server.Start();

After connecting with a client the server.Receive() Method starts right after. So data can already reveive data.

Sending data:

server.send();
