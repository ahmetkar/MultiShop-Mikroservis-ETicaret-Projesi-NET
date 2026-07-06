using Microsoft.AspNetCore.Authorization;
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




        public async Task<(bool,string)> RefundPayment(RefundPaymentDto refundPaymentDto, CancellationToken cancellationToken)
        {
            var payment = await _paymentContext.PaymentInfos.FirstOrDefaultAsync(x => x.OrderingId == refundPaymentDto.OrderingId, cancellationToken);

            if (payment is null)
            {
                return (false,"Ödeme iadesi başarısız");
            }

            if (payment.IsRefunded)
            {
                return (true,"Ödeme zaten geri yapılmış");
            }

            payment.IsRefunded = true;

            await _paymentContext.SaveChangesAsync(cancellationToken);

            return (true,"Ödeme iadesi başarıyla işleme alındı.");

        }

        public async Task<ResultCreatePaymentDto> AddPayment(CreatePaymentDto createPaymentDto,CancellationToken cancellationToken = default)
        {

            var orderSnapshot = await _paymentContext.PaymentOrderSnapshots.FirstOrDefaultAsync(x=>x.OrderingId == createPaymentDto.OrderingId,cancellationToken);
            
            if(orderSnapshot is null)
            {
                return new ResultCreatePaymentDto ();
            }

            if (orderSnapshot.IsSuccessful)
            {
                //sonra eklencek
                return new ResultCreatePaymentDto();
            }
            
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


            var paymentInfo = new PaymentInfo()
            {
                UserId = createPaymentDto.UserId,
                OrderingId = createPaymentDto.OrderingId,
                PaymentTotal = createPaymentDto.PaymentTotal,
                PaymentType = createPaymentDto.PaymentType,
                IsSuccessful = true,
                CardInfo = cardInfos
            };
            await _paymentContext.PaymentInfos.AddAsync(paymentInfo,cancellationToken);

            orderSnapshot.IsSuccessful = true;
            var change = await _paymentContext.SaveChangesAsync(cancellationToken);
            if (change >= 1)
            {
                return new ResultCreatePaymentDto()
                {
                    OrderingId = paymentInfo.OrderingId,
                    PaymentTotal = paymentInfo.PaymentTotal,
                    CardInfoId = paymentInfo.CardInfoId,
                    UserId = paymentInfo.UserId,
                    PaymentId = paymentInfo.Id
                };

            }
            return new ResultCreatePaymentDto();
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
