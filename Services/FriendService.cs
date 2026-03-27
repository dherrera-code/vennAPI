using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vennAPI.Context;
using vennAPI.Models;
using vennAPI.Models.DTO;

namespace vennAPI.Services
{
    public class FriendService(DataContext dataContext)
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<ActionResult<Friend>> SendFriendRequest(int requesterId, int receiverId)
        {
            //create an instance of Friend's model and make the status pending!
            //First check if a row already exist 
            //  if row already exist then return : return BadRequest!!!
            //Otherwise, create an instance
            bool doesEntryExist = await DoesFriendEntryExist(requesterId, receiverId);
            if (doesEntryExist)
            {
                return null;
            }
            Friend friendEntry = new();
            friendEntry.RequesterId = requesterId;
            friendEntry.ReceiverId = receiverId;
            friendEntry.Status = 0;

            return friendEntry;
        }

        public async Task<ActionResult<List<Friend>>> GetPendingFriendAsync(int userId)
        {

            var list = await _dataContext.Friends.Where(entries => entries.ReceiverId == userId).ToListAsync();
            return list;
        }

        public ActionResult<Friend> GetAcceptedFriendAsync(int userId)
        {
            throw new NotImplementedException();
        }
        // The receiver of the friend request must accept the friend request to change status.
        // This endpoint will handle the function!
        public ActionResult<Friend> AddFriendStatus(int Id)
        {
            // bool entry = await DoesFriendEntryExist(Id, OtherId);
            throw new NotImplementedException();
        }

        // * Helper Functions
        private async Task<bool> DoesFriendEntryExist(int userId, int receiverId)
        {
            var friendEntry = await _dataContext.Friends.Where(entry => entry.RequesterId == userId && entry.ReceiverId == receiverId).ToListAsync();
            if(friendEntry == null || friendEntry.Count == 0)
            {
                friendEntry = await _dataContext.Friends.Where(entry => entry.RequesterId == receiverId && entry.ReceiverId == userId).ToListAsync();
                if(friendEntry == null || friendEntry.Count == 0)
                {
                    return false;
                }
            }
            return true;

            // return await _dataContext.Friends.Where(friendEntry => friendEntry.RequesterId == userId).ToListAsync();
        }
    }
}