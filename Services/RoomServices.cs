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
    public class RoomServices
    {
        private readonly DataContext _dataContext;
        public RoomServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> AddNewRoomAsync(RoomModel room)
        {
            await _dataContext.Rooms.AddAsync(room);
            return await _dataContext.SaveChangesAsync() != 0;
        }
        public async Task<IEnumerable<RoomModel>> GetAllRoomsAsync() => await _dataContext.Rooms.ToListAsync();
        public async Task<RoomModel> GetRoomByRoomIdAsync(int roomId)
        {
            return await _dataContext.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);
        }

        public async Task<ActionResult<IEnumerable<RoomModel>>> GetAllRooms()
        {
            return await _dataContext.Rooms.AsNoTracking().ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<RoomModel>>> GetRoomsByUserIdAsync(int userId)
        {
            var rooms = await _dataContext.Rooms.Where(room => room.UserId == userId).AsNoTracking().ToListAsync();
            return rooms;
        }

        public async Task<ActionResult<RoomModel>> UpdateRoomAsync(int id, RoomModel updatedRoom)
        {
            var findRoom = await GetRoomByRoomIdAsync(id);

            if(findRoom == null) return null;

            findRoom.Title = updatedRoom.Title;
            findRoom.Category = updatedRoom.Category;
            findRoom.EventDate = updatedRoom.EventDate;
            findRoom.IsRoomActive = updatedRoom.IsRoomActive;

            _dataContext.Rooms.Update(findRoom);
            await _dataContext.SaveChangesAsync();
            return findRoom;
        }

              // Methods for room members!

        public async Task<ActionResult<RoomMemberDTO>> InviteMemberToRoom(RoomMemberDTO newRoomMember)
        {
            // Need to check if instance already Exist!!!
            var result = DoesInviteInstanceExist(newRoomMember.RoomModelId, newRoomMember.MemberId);
            if(result == null) return null;
            
            RoomMember invitedMember = new();
            invitedMember.RoomModelId = newRoomMember.RoomModelId;
            invitedMember.IsAccepted = false;
            invitedMember.UserModelId = newRoomMember.MemberId;
            
            await _dataContext.RoomMembers.AddAsync(invitedMember);
            await _dataContext.SaveChangesAsync();
            return newRoomMember;
        }

        private async Task<RoomMember> DoesInviteInstanceExist(int roomModelId, int memberId)
        {
            return await _dataContext.RoomMembers.SingleOrDefaultAsync(invite => invite.RoomModelId == roomModelId && invite.UserModelId == memberId);
        }

        public async Task<ActionResult<IEnumerable<RoomMember>>> GetAllJoinedMembersByRoom(int roomId)
        {
            return await _dataContext.RoomMembers.Where(item => item.RoomModelId == roomId && !item.IsDeleted).ToListAsync();
            
        }

        public async Task<ActionResult<bool>> ChangeMemberStatusToAccepted(RoomMemberDTO roomMember)
        {
            RoomMember member = await DoesRoomMemberInstanceExist(roomMember);
            if(member == null) return null;

            member.IsAccepted = true;
            _dataContext.Update(member);
            return await _dataContext.SaveChangesAsync() != 0; 

        }

        private async Task<RoomMember> DoesRoomMemberInstanceExist(RoomMemberDTO roomMember)
        {
            return await _dataContext.RoomMembers.SingleOrDefaultAsync(member => member.RoomModelId == roomMember.RoomModelId && member.UserModelId == roomMember.MemberId);

        }

        public async Task<bool> RemoveMemberFromRoom(RoomMemberDTO memberToRemove)
        {
            RoomMember member = await DoesRoomMemberInstanceExist(memberToRemove);

            if(member == null) return false;
            member.IsDeleted = true;
            _dataContext.Update(member);
            return await _dataContext.SaveChangesAsync() != 0;
        }

  

    }
}