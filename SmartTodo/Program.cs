using Microsoft.EntityFrameworkCore;
using SmartTodo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    // You can also create a more open policy for development if needed
    options.AddPolicy("AllowAnyOrigin",
        policy =>
        {
            policy.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseInMemoryDatabase("TodoList"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create a scope to access the service provider
using (var scope = app.Services.CreateScope())
{
    // Get the TodoDbContext instance
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();

    // Ensure the database is created
    dbContext.Database.EnsureCreated();

    // Check if there are already any todos
    if (!dbContext.Todos.Any())
    {
        // Add 20 sample Todo items
        dbContext.Todos.AddRange(
            new Todo { Title = "Grocery Shopping", Description = "Buy milk, eggs, and bread", Completed = false },
            new Todo { Title = "Pay Bills", Description = "Pay rent and utilities", Completed = false },
            new Todo { Title = "Schedule Doctor Appointment", Description = "Book a checkup for next week", Completed = false },
            new Todo { Title = "Write Blog Post", Description = "Draft the article for Friday", Completed = false },
            new Todo { Title = "Clean the House", Description = "Vacuum and mop the floors", Completed = false },
            new Todo { Title = "Book Flight", Description = "Book flight tickets for the holiday trip", Completed = false },
            new Todo { Title = "Learn a New Language", Description = "Spend 30 minutes on Duolingo", Completed = true },
            new Todo { Title = "Prepare Presentation", Description = "Create slides for the team meeting", Completed = false },
            new Todo { Title = "Walk the Dog", Description = "Take the dog for a 30-minute walk", Completed = true },
            new Todo { Title = "Read a Book", Description = "Read at least 20 pages", Completed = true },
            new Todo { Title = "Fix the Leaky Faucet", Description = "Call the plumber or try to fix it", Completed = false },
            new Todo { Title = "Renew Subscription", Description = "Renew the Netflix subscription", Completed = false },
            new Todo { Title = "Buy Birthday Gift", Description = "Buy a gift for John's birthday", Completed = false },
            new Todo { Title = "Attend Meeting", Description = "Attend the project kickoff meeting", Completed = false },
            new Todo { Title = "Water the Plants", Description = "Water all the indoor plants", Completed = true },
            new Todo { Title = "Plan Weekend Trip", Description = "Decide on a destination and book accommodation", Completed = false },
            new Todo { Title = "Exercise", Description = "Go for a run or hit the gym", Completed = true },
            new Todo { Title = "Review Code", Description = "Review the pull request from the team", Completed = false },
            new Todo { Title = "Bake a Cake", Description = "Bake a cake for the party", Completed = false },
            new Todo { Title = "Learn a new recipe", Description = "Try cooking something new", Completed = true }
            );

        // Save the changes to the database
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAnyOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
