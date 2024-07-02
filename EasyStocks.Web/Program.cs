

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEasyStockAppDbContext, EasyStockAppDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Dependency Injections
builder.Services.AddDbContext<EasyStockAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

//builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
//    .AddEntityFrameworkStores<EasyStockAppDbContext>()
//    .AddDefaultTokenProviders();

//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireDigit = true;
//    options.Password.RequiredLength = 8;
//    options.Password.RequireNonAlphanumeric = true;
//    options.Password.RequireUppercase = true;
//    options.Password.RequireLowercase = false;
//    options.Password.RequiredUniqueChars = 1;

//    //// Lockout settings
//    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//    //options.Lockout.MaxFailedAccessAttempts = 5;
//    //options.Lockout.AllowedForNewUsers = true;

//    //// User settings
//    //options.User.AllowedUserNameCharacters =
//    //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//    //options.User.RequireUniqueEmail = true;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
