using Explorer.Payments.API.Dtos.Gifts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Public
{
    public interface IGiftsService
    {
        GiftDto CreateGift(long donorId, CreateGiftDto request);
        GiftDto? GetById(long id);
        List<TouristFriendDto> GetTouristFriends(long userId);
    }
}
