using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cinemaTask.Migrations
{
    /// <inheritdoc />
    public partial class addDataToActorsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            INSERT INTO Actors (Name, Img) VALUES
            ('Lock Tuer', 'ElitProin.doc'),
            ('Lindsey Spencers', 'BlanditNam.mov'),
            ('Roshelle Camber', 'Nisl.mpeg'),
            ('Lorianna Kyttor', 'NullaJustoAliquam.xls'),
            ('Dewie Falkingham', 'VelEnim.mov'),
            ('Denyse Leyzell', 'Luctus.mpeg'),
            ('Lukas Studdeard', 'Amet.avi'),
            ('Burk Lettley', 'Venenatis.tiff'),
            ('Piotr Capper', 'BibendumFelisSed.ppt'),
            ('Aguistin Rogier', 'Convallis.mp3'),
            ('Erna Batting', 'UllamcorperAugue.mov'),
            ('Ichabod Frostdick', 'LobortisSapienSapien.ppt'),
            ('Karon Najara', 'Erat.mov'),
            ('Delaney Humfrey', 'DuisAcNibh.png'),
            ('Hedvige Werndly', 'AtDolor.mpeg'),
            ('Hermione Kalinsky', 'Molestie.xls'),
            ('Mia Frowd', 'Congue.pdf'),
            ('Eliza Adamo', 'NequeVestibulum.mp3'),
            ('Ericka Goodings', 'Nisl.mov'),
            ('Marta Sapsforde', 'InLacus.ppt'),
            ('Ginny O''Dea', 'FelisDonecSemper.xls'),
            ('Sarette Bristowe', 'DonecOdio.ppt'),
            ('Meris Tremble', 'NullamSitAmet.tiff'),
            ('Ives Askam', 'RutrumAc.xls'),
            ('Marja Larman', 'NullamVariusNulla.avi'),
            ('Wanids Burnapp', 'In.ppt'),
            ('Cris McCahill', 'EtMagnisDis.mpeg'),
            ('Roddy Blanque', 'Posuere.mp3'),
            ('Alethea Adolfson', 'SapienQuisLibero.doc'),
            ('Vasili Zmitrichenko', 'Duis.mp3'),
            ('Renell Hensmans', 'PrimisInFaucibus.jpeg'),
            ('Evey MacVay', 'OdioDonec.xls'),
            ('Foss Timny', 'QuisTortorId.png'),
            ('Barby Bointon', 'PorttitorLorem.avi'),
            ('Dorisa Cumbers', 'Velit.avi'),
            ('Kamila Brigginshaw', 'NonQuamNec.ppt'),
            ('Sharla Harg', 'NuncPurus.jpeg'),
            ('Bevin Pobjoy', 'NullaFacilisi.mp3'),
            ('Brana Jarrett', 'PorttitorLacusAt.xls'),
            ('Norma Burley', 'FaucibusCursus.xls');
            ");
        }
        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE Actors");

        }
    }
}
