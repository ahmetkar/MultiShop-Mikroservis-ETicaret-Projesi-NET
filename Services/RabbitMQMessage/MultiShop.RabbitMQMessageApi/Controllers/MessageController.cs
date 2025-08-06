using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime.InteropServices;
using System.Text;

namespace MultiShop.RabbitMQMessageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateMessage()
        {


            ConnectionFactory connFactory = new ConnectionFactory()
            {
                HostName = "localhost",

            };

            await using IConnection conn = await connFactory.CreateConnectionAsync();

            await using IChannel channel = await conn.CreateChannelAsync();



            await channel.QueueDeclareAsync("Kuyruk1", false, false, false, null);

            string message = "Merhaba bir kuyruk mesajı bu";
            byte[] body = Encoding.UTF8.GetBytes(message);



            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "Kuyruk1",
                mandatory: false,
                basicProperties: new BasicProperties(),
                body: body);


            return Ok("Mesajınız alındı");
        }


        [HttpPost("ReadMessage")]
        public async Task<IActionResult> ReadMessage()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            await using IConnection connection = await factory.CreateConnectionAsync();
            await using IChannel channel = await connection.CreateChannelAsync();

            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);

            string msgstr = "";
            consumer.ReceivedAsync += async (model,body) =>
            {
                var byteMsg = body.Body.ToArray();
                msgstr = Encoding.UTF8.GetString(byteMsg);

                
            };

            await channel.BasicConsumeAsync("Kuyruk1",false,consumer:consumer);

            return Ok(msgstr);




        }
    }

}