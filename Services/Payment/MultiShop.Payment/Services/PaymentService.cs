using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.Payment.DAL.Context;
using MultiShop.Payment.DAL.Entities;
using MultiShop.Payment.DTOs;

namespace MultiShop.Payment.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly PaymentContext _paymentContext;

        public PaymentService(PaymentContext paymentContext)
        {
            _paymentContext = paymentContext;
        }


        public async Task<List<ResultPaymentDto>> GetAllPaymentByUserId(string id)
        {
            var paymentinfoslist = _paymentContext.PaymentInfos.Include(x => x.CardInfo).Where(x => x.UserId == id).ToList();

            var list = new List<ResultPaymentDto>();
            foreach (var paymentinfos in paymentinfoslist)
            {

                var payment = new ResultPaymentDto()
                {
                    Id = paymentinfos.Id,
                    UserId = paymentinfos.UserId,
                    OrderingId = paymentinfos.OrderingId,
                    PaymentTotal = paymentinfos.PaymentTotal,
                    PaymentType = paymentinfos.PaymentType,
                    CardType = paymentinfos.CardInfo.CardType,
                    CardBankName = paymentinfos.CardInfo.CardBankName,
                    CardBrand = paymentinfos.CardInfo.CardBrand,
                    CardInfoId = paymentinfos.CardInfo.CardInfoId,
                    LastDateMonth = paymentinfos.CardInfo.LastDateMonth,
                    LastDateYear = paymentinfos.CardInfo.LastDateYear,
                    OwnerName = paymentinfos.CardInfo.OwnerName,
                    OwnerSurname = paymentinfos.CardInfo.OwnerSurname,
                    LastFourNumber = paymentinfos.CardInfo.LastFourNumber
                };
                list.Add(payment);
            }
            return list;
        }


        public async Task<ResultPaymentDto> GetPaymentByOrderingId(int id)
        {
            var paymentinfos = _paymentContext.PaymentInfos.Include(x => x.CardInfo).Where(x => x.OrderingId == id).LastOrDefault();

            return new ResultPaymentDto()
            {
                Id = paymentinfos.Id,
                UserId = paymentinfos.UserId,
                OrderingId = paymentinfos.OrderingId,
                PaymentTotal = paymentinfos.PaymentTotal,
                PaymentType = paymentinfos.PaymentType,
                CardType = paymentinfos.CardInfo.CardType,
                CardBankName = paymentinfos.CardInfo.CardBankName,
                CardBrand = paymentinfos.CardInfo.CardBrand,
                CardInfoId = paymentinfos.CardInfo.CardInfoId,
                LastDateMonth = paymentinfos.CardInfo.LastDateMonth,
                LastDateYear = paymentinfos.CardInfo.LastDateYear,
                OwnerName = paymentinfos.CardInfo.OwnerName,
                OwnerSurname = paymentinfos.CardInfo.OwnerSurname,
                LastFourNumber = paymentinfos.CardInfo.LastFourNumber
            };
        }


   
        public async Task<bool> AddPayment(CreatePaymentDto createPaymentDto)
        {

            CardInfo cardInfos = new CardInfo()
            {
                CardType = createPaymentDto.CardType,
                CardBankName = createPaymentDto.CardBankName,
                CardBrand = createPaymentDto.CardBrand,
                LastDateMonth = createPaymentDto.LastDateMonth,
                LastDateYear = createPaymentDto.LastDateYear,
                OwnerName = createPaymentDto.OwnerName,
                OwnerSurname = createPaymentDto.OwnerSurname,
                LastFourNumber = createPaymentDto.LastFourNumber
            };

            _paymentContext.CardInfos.Add(cardInfos);
            _paymentContext.SaveChanges();

            _paymentContext.PaymentInfos.Add(new PaymentInfo()
            {
                UserId = createPaymentDto.UserId,
                OrderingId = createPaymentDto.OrderingId,
                PaymentTotal = createPaymentDto.PaymentTotal,
                PaymentType = createPaymentDto.PaymentType,
                CardInfoId = cardInfos.CardInfoId
            });
            var change = _paymentContext.SaveChanges();
            if (change >= 1) return true;
            return false;
        }

        public async Task<bool> CancelPaymentByOrderingId(int id)
        {
            var deleted = _paymentContext.PaymentInfos.Where(x=>x.OrderingId == id).FirstOrDefault();
            var deleted2 = _paymentContext.CardInfos.Where(x => x.CardInfoId == deleted.CardInfoId).FirstOrDefault();
            _paymentContext.PaymentInfos.Remove(deleted!);
            _paymentContext.CardInfos.Remove(deleted2!);
            var change = _paymentContext.SaveChanges();
            if (change >= 1) return true;
            return false;

        }


    }
}
