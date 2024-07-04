
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<IEasyStockAppDbContext, EasyStockAppDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEasyStockServices();

// Dependency Injections
//builder.Services.AddDbContext<EasyStockAppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

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

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
