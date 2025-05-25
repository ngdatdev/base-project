//using BaseApiReference.Abstractions.Notification.SignalR;
//using Microsoft.AspNetCore.SignalR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Notification.SignalR;

///// <summary>
///// This is a sample implementation of IChatHandler
///// </summary>
//public class ChatHandler : IChatHandler
//{
//    private readonly IHubContext<ChatHub> _hubContext;

//    public ChatHandler(IHubContext<ChatHub> hubContext)
//    {
//        _hubContext = hubContext;
//    }

//    public async Task<bool> SendMessageAsync(ChatMessage chatMessage)
//    {
//        var isChatRoomExperid =
//            await _unitOfWork.CreateChatContentRepository.IsChatRoomExperiedQueryAsync(
//                chatRoomId: Guid.Parse(input: chatMessage.ChatRoomId),
//                cancellationToken: default
//            );

//        if (isChatRoomExperid)
//        {
//            return false;
//        }

//        var createdTime = DateTime.Now;
//        var chatContentId = Guid.NewGuid();
//        var chatContent = new ChatContent()
//        {
//            Id = Guid.Parse(input: chatMessage.ChatContentId),
//            TextContent = chatMessage.Message,
//            Assets = chatMessage
//                .ImageUrls.Select(asset => new Asset()
//                {
//                    Id = Guid.NewGuid(),
//                    FilePath = asset,
//                    FileName = asset,
//                    Type = "image",
//                })
//                .ToList(),
//            CreatedAt = createdTime,
//            CreatedBy = Guid.Parse(input: chatMessage.SenderId),
//            SenderId = Guid.Parse(input: chatMessage.SenderId),
//            ChatRoomId = Guid.Parse(input: chatMessage.ChatRoomId),
//        };

//        var dbResult = await _unitOfWork.CreateChatContentRepository.AddChatContentCommandAsync(
//            chatContent,
//            createdTime,
//            Guid.Parse(input: chatMessage.ChatRoomId),
//            cancellationToken: default
//        );

//        if (!dbResult)
//        {
//            return false;
//        }

//        try
//        {
//            var connectIdReceivers = ChatHub.GetConnectionIds(chatMessage.ReceiverId);

//            var connectIdSenders = ChatHub.GetConnectionIds(chatMessage.SenderId);

//            if (Equals(connectIdSenders, default))
//            {
//                return false;
//            }

//            if (!Equals(connectIdReceivers, default))
//            {
//                connectIdSenders?.AddRange(connectIdReceivers);
//            }

//            await _hubContext
//                .Clients.Clients(connectIdSenders)
//                .SendAsync(
//                    "ReceiveMessage",
//                    chatMessage.ChatContentId,
//                    chatMessage.SenderId,
//                    chatMessage.Message,
//                    chatMessage.ChatRoomId,
//                    chatMessage.ImageUrls,
//                    chatMessage.VideoUrls,
//                    createdTime
//                );

//            await _hubContext
//                .Clients.Clients(connectIdSenders)
//                .SendAsync("ReceiveChatRoom", chatMessage.ChatRoomId, createdTime);

//            return true;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error sending message: {ex.Message}");
//            return false;
//        }
//    }

//    public async Task SendTypingAsync(string senderId, string receiverId)
//    {
//        var connectIdReceivers = ChatHub.GetConnectionIds(receiverId);

//        if (Equals(connectIdReceivers, default))
//        {
//            return;
//        }

//        await _hubContext.Clients.Clients(connectIdReceivers).SendAsync("ReceiveTyping", senderId);
//    }

//    public async Task SendStopTypingAsync(string senderId, string receiverId)
//    {
//        var connectIdReceivers = ChatHub.GetConnectionIds(receiverId);

//        await _hubContext
//            .Clients.Clients(connectIdReceivers)
//            .SendAsync("ReceiveStopTyping", senderId);
//    }

//    public async Task<bool> SendRemovedMessageAsync(
//        string senderId,
//        string receiverId,
//        string chatContentId
//    )
//    {
//        var connectIdReceivers = ChatHub.GetConnectionIds(receiverId);

//        var connectIdSenders = ChatHub.GetConnectionIds(senderId);

//        if (Equals(connectIdSenders, default))
//        {
//            return false;
//        }

//        if (!Equals(connectIdReceivers, default))
//        {
//            connectIdSenders?.AddRange(connectIdReceivers);
//        }

//        await _hubContext
//            .Clients.Clients(connectIdSenders)
//            .SendAsync("ReceiveRemovedMessage", senderId, chatContentId);

//        return true;
//    }
//}
