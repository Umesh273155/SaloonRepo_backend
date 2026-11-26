using Microsoft.EntityFrameworkCore;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;
using SalonSphere.Core.Entities;
using SalonSphere.Infrastructure.Data;
using System.Globalization;

namespace SalonSphere.Infrastructure.Services
{
    public class SalonService : ISalonService
    {
        private readonly ApplicationDbContext _context;
        private static readonly string[] Days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        public SalonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<BusinessHoursDto>>> GetBusinessHoursAsync(int userId)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
            {
                return new ApiResponse<List<BusinessHoursDto>>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = null
                };
            }

            if (!user.SalonId.HasValue)
            {
                return new ApiResponse<List<BusinessHoursDto>>
                {
                    Success = false,
                    Message = "No salon is associated with this user.",
                    Data = null
                };
            }

            var salonId = user.SalonId.Value;

            var hours = await _context.SalonBusinessHours
                .Where(x => x.SalonId == salonId)
                .ToListAsync();

            foreach (var day in Days)
            {
                if (!hours.Any(x => x.Day == day))
                {
                    var index = Array.IndexOf(Days, day);

                    var newHour = new SalonBusinessHour
                    {
                        SalonId = salonId,
                        Day = day,
                        IsOpen = index != 6,
                        OpenTime = TimeSpan.FromHours(9),
                        CloseTime = TimeSpan.FromHours(20)
                    };

                    hours.Add(newHour);
                    _context.SalonBusinessHours.Add(newHour);
                }
            }

            await _context.SaveChangesAsync();

            var result = hours
                .OrderBy(x => Array.IndexOf(Days, x.Day))
                .Select(x => new BusinessHoursDto
                {
                    Day = x.Day,
                    IsOpen = x.IsOpen,
                    OpenTime = x.OpenTime.ToString(@"hh\:mm"),
                    CloseTime = x.CloseTime.ToString(@"hh\:mm")
                })
                .ToList();

            return new ApiResponse<List<BusinessHoursDto>>
            {
                Success = true,
                Message = "Business hours fetched successfully.",
                Data = result
            };
        }

        public async Task<ApiResponse<bool>> UpdateBusinessHoursAsync(int userId,List<BusinessHoursDto> hours)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = false
                };
            }

            if (!user.SalonId.HasValue)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "No salon is associated with this user.",
                    Data = false
                };
            }

            if (hours == null || hours.Count != 7 || hours.Select(x => x.Day).Distinct().Count() != 7 || hours.Any(x => !Days.Contains(x.Day)))
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Exactly seven unique weekdays are required.",
                    Data = false
                };
            }

            foreach (var item in hours)
            {
                if (!TimeSpan.TryParseExact(
                        item.OpenTime,
                        @"hh\:mm",
                        CultureInfo.InvariantCulture,
                        out var openTime) ||
                    !TimeSpan.TryParseExact(
                        item.CloseTime,
                        @"hh\:mm",
                        CultureInfo.InvariantCulture,
                        out var closeTime))
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = $"Invalid time format for {item.Day}.",
                        Data = false
                    };
                }

                if (item.IsOpen && openTime >= closeTime)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = $"Closing time must be after opening time for {item.Day}.",
                        Data = false
                    };
                }
            }

            var salonId = user.SalonId.Value;

            var existing = await _context.SalonBusinessHours
                .Where(x => x.SalonId == salonId)
                .ToListAsync();

            foreach (var item in hours)
            {
                var record = existing.FirstOrDefault(x => x.Day == item.Day);

                if (record == null)
                {
                    record = new SalonBusinessHour
                    {
                        SalonId = salonId,
                        Day = item.Day
                    };

                    _context.SalonBusinessHours.Add(record);
                }

                record.IsOpen = item.IsOpen;

                record.OpenTime = TimeSpan.ParseExact(
                    item.OpenTime,
                    @"hh\:mm",
                    CultureInfo.InvariantCulture);

                record.CloseTime = TimeSpan.ParseExact(
                    item.CloseTime,
                    @"hh\:mm",
                    CultureInfo.InvariantCulture);
            }

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Business hours updated successfully.",
                Data = true
            };
        }

     
        public async Task<ApiResponse<SalonDetailsDto?>> GetMySalonAsync(int userId)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = null
                };
            }
            if (!user.SalonId.HasValue)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "No salon is associated with this user.",
                    Data = null
                };
            }
            var salon = await _context.Salons.AsNoTracking().Where(s => s.SalonId == user.SalonId.Value)
                .Select(s => new SalonDetailsDto
                {
                    SalonId = s.SalonId,
                    SalonName = s.SalonName,
                    SalonType = s.SalonType,
                    Description = s.Description,
                    LogoUrl = s.LogoUrl,
                    Phone = s.Phone,
                    Email = s.Email,
                    Address = s.Address,
                    City = s.City,
                    State = s.State,
                    Pincode = s.Pincode,
                    IsActive = s.IsActive,
                    Website = s.Website,
                }).FirstOrDefaultAsync();

            if (salon != null)
            {
                salon.OwnerName = user.FullName;
            }

            if (salon == null)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "Salon not found.",
                    Data = null
                };
            }

            return new ApiResponse<SalonDetailsDto?>
            {
                Success = true,
                Message = "Salon details fetched successfully.",
                Data = salon
            };
        }


        public async Task<ApiResponse<SalonDetailsDto?>> UpdateSalonAsync(int userId, UpdateSalonReqDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = null
                };
            }

            if (!user.SalonId.HasValue)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "No salon is associated with this user.",
                    Data = null
                };
            }

            var salon = await _context.Salons.FirstOrDefaultAsync(s => s.SalonId == user.SalonId.Value);
            if (salon == null)
            {
                return new ApiResponse<SalonDetailsDto?>
                {
                    Success = false,
                    Message = "Salon not found.",
                    Data = null
                };
            }

            salon.SalonName = request.SalonName;
            salon.SalonType = request.SalonType;
            salon.Description = request.Description;
            salon.Phone = request.Phone;
            salon.Email = request.Email;
            salon.Address = request.Address;
            salon.City = request.City;
            salon.State = request.State;
            salon.Pincode = request.Pincode;
            salon.Website = request.Website;
            user.FullName = request.OwnerName ?? user.FullName;

            await _context.SaveChangesAsync();
            var response = new SalonDetailsDto
            {
                SalonId = salon.SalonId,
                SalonName = salon.SalonName,
                SalonType = salon.SalonType,
                Description = salon.Description,
                LogoUrl = salon.LogoUrl,
                Phone = salon.Phone,
                Email = salon.Email,
                Address = salon.Address,
                City = salon.City,
                State = salon.State,
                Pincode = salon.Pincode,
                IsActive = salon.IsActive,
                Website = salon.Website,
                OwnerName = user.FullName,
            };

            return new ApiResponse<SalonDetailsDto?>
            {
                Success = true,
                Message = "Salon details updated successfully.",
                Data = response
            };
        }
        public async Task<ApiResponse<AppointmentPreferenceDto?>> GetAppointmentPreferencesAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
            {
                return new ApiResponse<AppointmentPreferenceDto?>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = null
                };
            }

            if (!user.SalonId.HasValue)
            {
                return new ApiResponse<AppointmentPreferenceDto?>
                {
                    Success = false,
                    Message = "No salon is associated with this user.",
                    Data = null
                };
            }

            var salonId = user.SalonId.Value;
            var preference = await _context.AppointmentPreferences.FirstOrDefaultAsync(x => x.SalonId == salonId);

            if (preference == null)
            {
                preference = new AppointmentPreference
                {
                    SalonId = salonId,
                    AppointmentDuration = 30,
                    AdvanceBookingDays = 30,
                    CancellationHours = 2,
                    TimeZone = "Asia/Kolkata",
                    OnlineBooking = true,
                    RequireConfirmation = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.AppointmentPreferences.Add(preference);

                await _context.SaveChangesAsync();
            }

            var result = new AppointmentPreferenceDto
            {
                AppointmentPreferenceId = preference.AppointmentPreferenceId,
                AppointmentDuration = preference.AppointmentDuration,
                AdvanceBookingDays = preference.AdvanceBookingDays,
                CancellationHours = preference.CancellationHours,
                TimeZone = preference.TimeZone,
                OnlineBooking = preference.OnlineBooking,
                RequireConfirmation = preference.RequireConfirmation
            };

            return new ApiResponse<AppointmentPreferenceDto?>
            {
                Success = true,
                Message = "Appointment preferences fetched successfully.",
                Data = result
            };
        }


        public async Task<ApiResponse<AppointmentPreferenceDto>> UpdateAppointmentPreferencesAsync(int userId,UpdateAppointmentPreferenceDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
            {
                return new ApiResponse<AppointmentPreferenceDto>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = null
                };
            }

            if (!user.SalonId.HasValue)
            {
                return new ApiResponse<AppointmentPreferenceDto>
                {
                    Success = false,
                    Message = "No salon is associated with this user.",
                    Data = null
                };
            }

            if (request == null)
            {
                return new ApiResponse<AppointmentPreferenceDto>
                {
                    Success = false,
                    Message = "Invalid appointment preferences.",
                    Data = null
                };
            }

            var allowedDurations = new[] { 15, 30, 45, 60, 90 };
            var allowedBookingDays = new[] { 7, 15, 30, 60, 90 };
            var allowedCancellationHours = new[] { 1, 2, 6, 12, 24 };
            var allowedTimeZones = new[]
            {
                "Asia/Kolkata",
                "Asia/Dubai",
                "Asia/Singapore",
                "Europe/London"
            };

            if (!allowedDurations.Contains(request.AppointmentDuration) || !allowedBookingDays.Contains(request.AdvanceBookingDays) || !allowedCancellationHours.Contains(request.CancellationHours) || string.IsNullOrWhiteSpace(request.TimeZone) || !allowedTimeZones.Contains(request.TimeZone))
            {
                return new ApiResponse<AppointmentPreferenceDto>
                {
                    Success = false,
                    Message = "Invalid appointment preference values.",
                    Data = null
                };
            }

            var salonId = user.SalonId.Value;

            var preference = await _context.AppointmentPreferences.FirstOrDefaultAsync(x => x.SalonId == salonId);

            if (preference == null)
            {
                preference = new AppointmentPreference
                {
                    SalonId = salonId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.AppointmentPreferences.Add(preference);
            }

            preference.AppointmentDuration = request.AppointmentDuration;
            preference.AdvanceBookingDays = request.AdvanceBookingDays;
            preference.CancellationHours = request.CancellationHours;
            preference.TimeZone = request.TimeZone;
            preference.OnlineBooking = request.OnlineBooking;
            preference.RequireConfirmation = request.RequireConfirmation;
            preference.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            var result = new AppointmentPreferenceDto
            {
                AppointmentPreferenceId = preference.AppointmentPreferenceId,
                AppointmentDuration = preference.AppointmentDuration,
                AdvanceBookingDays = preference.AdvanceBookingDays,
                CancellationHours = preference.CancellationHours,
                TimeZone = preference.TimeZone,
                OnlineBooking = preference.OnlineBooking,
                RequireConfirmation = preference.RequireConfirmation
            };
            return new ApiResponse<AppointmentPreferenceDto>
            {
                Success = true,
                Message = "Appointment preferences updated successfully.",
                Data = result
            };
        }

        public async Task<ApiResponse<NotificationPreferencesResDto?>>GetNotificationPreferencesAsync(int userId)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (user == null)
                {
                    return new ApiResponse<NotificationPreferencesResDto?>
                    {
                        Success = false,
                        Message = "User not found",
                        Data = null
                    };
                }

                var preferences = await _context.SalonNotificationPreferences
                    .FirstOrDefaultAsync(x => x.SalonId == user.SalonId);

                if (preferences == null)
                {
                    preferences = new SalonNotificationPreference
                    {
                        SalonId = user.SalonId!.Value,
                        AppointmentNotifications = true,
                        PaymentNotifications = true,
                        StockNotifications = true,
                        EmailNotifications = true,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.SalonNotificationPreferences.Add(preferences);
                    await _context.SaveChangesAsync();
                }

                var response = new NotificationPreferencesResDto
                {
                    AppointmentNotifications = preferences.AppointmentNotifications,
                    PaymentNotifications = preferences.PaymentNotifications,
                    StockNotifications = preferences.StockNotifications,
                    EmailNotifications = preferences.EmailNotifications
                };

                return new ApiResponse<NotificationPreferencesResDto?>
                {
                    Success = true,
                    Message = "Notification preferences fetched successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<NotificationPreferencesResDto?>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<NotificationPreferencesResDto?>>UpdateNotificationPreferencesAsync(int userId,UpdateNotificationPreferencesReqDto request)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (user == null)
                {
                    return new ApiResponse<NotificationPreferencesResDto?>
                    {
                        Success = false,
                        Message = "User not found",
                        Data = null
                    };
                }

                var preferences = await _context.SalonNotificationPreferences
                    .FirstOrDefaultAsync(x => x.SalonId == user.SalonId);

                if (preferences == null)
                {
                    preferences = new SalonNotificationPreference
                    {
                        SalonId = user.SalonId!.Value
                    };

                    _context.SalonNotificationPreferences.Add(preferences);
                }

                preferences.AppointmentNotifications = request.AppointmentNotifications;
                preferences.PaymentNotifications = request.PaymentNotifications;
                preferences.StockNotifications = request.StockNotifications;
                preferences.EmailNotifications = request.EmailNotifications;
                preferences.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var response = new NotificationPreferencesResDto
                {
                    AppointmentNotifications = preferences.AppointmentNotifications,
                    PaymentNotifications = preferences.PaymentNotifications,
                    StockNotifications = preferences.StockNotifications,
                    EmailNotifications = preferences.EmailNotifications
                };

                return new ApiResponse<NotificationPreferencesResDto?>
                {
                    Success = true,
                    Message = "Notification preferences updated successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<NotificationPreferencesResDto?>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }


    }
}
