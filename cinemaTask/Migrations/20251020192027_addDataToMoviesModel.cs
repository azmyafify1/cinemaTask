using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cinemaTask.Migrations
{
    /// <inheritdoc />
    public partial class addDataToMoviesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Movies (Name, Description, Price, Status, DateTime, MainImg, CategoryId, CinemaId) VALUES 
                ('Unidentified Flying Oddball', 'Suspendisse potenti. Cras in purus eu maga vulputate luctus...', 642.32, 1, '2010-07-16T00:00:00Z', '1.png', 1, 6),
                ('Tripping the Rift: The Movie', 'Duis bibendum. Morbi non quam nec dui luctus rutrum...', 269.36, 1, '2010-07-16T00:00:00Z', '2.png', 6, 8),
                ('The Fan', 'Donec quis orci eget orci vehicula condimentum...', 550.31, 0, '2010-07-16T00:00:00Z', '6.png', 9, 6),
                ('The Man from Snowy River', 'Phasellus id sapien in sapien iaculis congue...', 456.11, 0, '2010-07-16T00:00:00Z', '7.png', 2, 3),
                ('Tea For Two', 'Donec posuere metus vitae ipsum. Aliquam non mauris...', 644.87, 1, '2010-07-16T00:00:00Z', '3.png', 4, 2),
                ('El Cid', 'In hac habitasse platea dictumst...', 349.86, 1, '2010-07-16T00:00:00Z', '7.png', 10, 8),
                ('3 Idiots', 'Quisque erat eros, viverra eget, congue eget...', 266.56, 0, '2010-07-16T00:00:00Z', '1.png', 7, 4),
                ('Mike Tyson: Undisputed Truth', 'Morbi porttitor lorem id ligula...', 560.08, 1, '2010-07-16T00:00:00Z', '8.png', 5, 8),
                ('Dead Men Walk', 'Donec ut dolor. Morbi vel lectus in quam fringilla rhoncus...', 776.12, 1, '2010-07-16T00:00:00Z', '8.png', 3, 9),
                ('Bodies, Rest & Motion', 'Cras mi pede, malesuada in, imperdiet et...', 186.63, 1, '2010-07-16T00:00:00Z', '1.png', 9, 7),
                ('Avatar: The Way of Water', 'Epic science fiction film...', 950.00, 1, '2022-12-16T00:00:00Z', '9.png', 11, 1),
                ('John Wick 4', 'Action-packed thriller...', 800.00, 1, '2023-03-24T00:00:00Z', '10.png', 12, 4),
                ('Oppenheimer', 'Historical drama directed by Christopher Nolan...', 700.00, 1, '2023-07-21T00:00:00Z', '11.png', 13, 2),
                ('The Dark Knight', 'Best Batman movie ever...', 600.00, 1, '2008-07-18T00:00:00Z', '12.png', 14, 5),
                ('Inception', 'Mind-bending sci-fi thriller...', 750.00, 1, '2010-07-16T00:00:00Z', '13.png', 15, 6),
                ('Titanic', 'Romantic historical drama...', 500.00, 1, '1997-12-19T00:00:00Z', '14.png', 16, 3),
                ('The Matrix', 'A computer hacker learns about reality...', 720.00, 1, '1999-03-31T00:00:00Z', '15.png', 17, 7),
                ('Joker', 'Psychological thriller...', 650.00, 1, '2019-10-04T00:00:00Z', '16.png', 18, 5),
                ('Dune', 'Sci-fi adventure based on Frank Herbert’s novel...', 880.00, 1, '2021-10-22T00:00:00Z', '17.png', 19, 8),
                ('Interstellar', 'Exploration through space and time...', 930.00, 1, '2014-11-07T00:00:00Z', '18.png', 20, 9);
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE Movies");

        }
    }
}
