using System;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace API.Data;

public class MessageRepository(DataContext context, IMapper mapper) : IMessageRepository
{
    public void AddMessage(Message message)
    {
        context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        context.Messages.Remove(message);
    }

    public async Task<Message?> GetMessage(int id)
    {
        return await context.Messages.FindAsync(id);
    }

    public Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = context.Messages
        .OrderByDescending(m => m.MessageSent)
        .AsQueryable();

        query = messageParams.Container switch
        {
            "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username),
            "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username),
            _ => query.Where(u => u.RecipientUsername == messageParams.Username && u.DateRead == null)
        };

        var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

        return PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber,
         messageParams.PageSize);
    }

    public Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
