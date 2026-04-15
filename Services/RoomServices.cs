using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var mainRoom = await _dataContext.Rooms.Include(mem => mem.Members).FirstOrDefaultAsync(room => roomId == room.RoomId);

            return mainRoom;
        }

        public async Task<ActionResult<IEnumerable<RoomModel>>> GetAllRooms()
        {
            return await _dataContext.Rooms.AsNoTracking()
            .Include(member => member.Members)
            .ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<RoomModel>>> GetRoomsByUserIdAsync(int userId)
        {
            var rooms = await _dataContext.Rooms.Where(room => room.UserId == userId).AsNoTracking().ToListAsync();
            return rooms;
        }

        public async Task<ActionResult<RoomModel>> UpdateRoomAsync(int id, RoomModel updatedRoom)
        {
            var findRoom = await GetRoomByRoomIdAsync(id);

            if (findRoom == null) return null;

            findRoom.Title = updatedRoom.Title;
            findRoom.Category = updatedRoom.Category;
            findRoom.EventDate = updatedRoom.EventDate;
            findRoom.IsRoomActive = updatedRoom.IsRoomActive;

            _dataContext.Rooms.Update(findRoom);
            await _dataContext.SaveChangesAsync();
            return findRoom;
        }

        // Methods for room members!

        public async Task<bool> InviteMemberToRoom(RoomMemberDTO newRoomMember)
        {
            // Need to check if instance already Exist!!!
            var invite = await DoesInviteInstanceExist(newRoomMember.RoomModelId, newRoomMember.MemberId);

            if (invite != null) return false;

            RoomMember invitedMember = new()
            {
                RoomModelId = newRoomMember.RoomModelId,
                IsAccepted = false,
                UserModelId = newRoomMember.MemberId,
                IsDeleted = false
            };

            await _dataContext.RoomMembers.AddAsync(invitedMember);
            return await _dataContext.SaveChangesAsync() != 0;

        }

        private async Task<RoomMember> DoesInviteInstanceExist(int roomModelId, int memberId)
        {
            return await _dataContext.RoomMembers.SingleOrDefaultAsync(invite => invite.RoomModelId == roomModelId && invite.UserModelId == memberId && !invite.IsDeleted);
        }

        public async Task<ActionResult<IEnumerable<Object>>> GetAllJoinedMembersByRoom(int roomId)
        {
            DateTime dayOfWeek = await _dataContext.Rooms.Where(r => r.RoomId == roomId).Select(date => date.EventDate).FirstOrDefaultAsync();

            return await _dataContext.RoomMembers.Where(item => item.RoomModelId == roomId && item.IsAccepted && !item.IsDeleted).Select(item => new
            {
                item.RoomModelId,
                item.UserModelId,
                item.IsAccepted,
                item.MemberInfo.UserId,
                item.MemberInfo.Username,
                item.MemberInfo.UserIcon,
                // add more data here for availability!
                // item.MemberInfo.Availability // this will return ALL member's availability!
                Availability = item.MemberInfo.Availability.Where(day => day.Day == dayOfWeek.DayOfWeek)

            })
            .ToListAsync();
        }

        public async Task<ActionResult<bool>> ChangeMemberStatusToAccepted(RoomMemberDTO roomMember)
        {
            RoomMember member = await DoesRoomMemberInstanceExist(roomMember);
            if (member == null) return null;

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

            if (member == null) return false;
            member.IsDeleted = true;
            _dataContext.Update(member);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<List<RoomMember>> GetPendingInvitesByUserId(int userId)
        {
            var invitationList = await _dataContext.RoomMembers.Where(item => item.UserModelId == userId && !item.IsAccepted && !item.IsDeleted).Include(item => item.Room).ToListAsync();

            return invitationList;
        }
    }
}