using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MixxFit.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Exercises_Name",
                table: "Exercises");

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "ExerciseCategoryId", "ExerciseType", "MuscleGroupId", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, 2, 1, 1, "Bench Press", null },
                    { 2, 2, 1, 1, "Incline Bench Press", null },
                    { 3, 2, 1, 1, "Decline Bench Press", null },
                    { 4, 2, 1, 1, "Close Grip Bench Press", null },
                    { 5, 2, 1, 1, "Floor Press", null },
                    { 6, 2, 1, 1, "Guillotine Press", null },
                    { 7, 2, 1, 2, "Bent Over Row", null },
                    { 8, 2, 1, 2, "Pendlay Row", null },
                    { 9, 2, 1, 2, "T-Bar Row", null },
                    { 10, 2, 1, 2, "Deadlift", null },
                    { 11, 2, 1, 2, "Rack Pull", null },
                    { 12, 2, 1, 2, "Seal Row", null },
                    { 13, 2, 1, 3, "Overhead Press", null },
                    { 14, 2, 1, 3, "Behind Neck Press", null },
                    { 15, 2, 1, 3, "Upright Row", null },
                    { 16, 2, 1, 3, "Front Raise", null },
                    { 17, 2, 1, 3, "Bradford Press", null },
                    { 18, 2, 1, 4, "Curl", null },
                    { 19, 2, 1, 4, "Preacher Curl", null },
                    { 20, 2, 1, 4, "Reverse Curl", null },
                    { 21, 2, 1, 4, "Skull Crusher", null },
                    { 22, 2, 1, 4, "Spider Curl", null },
                    { 23, 2, 1, 4, "EZ Bar Curl", null },
                    { 24, 2, 1, 4, "EZ Bar Skull Crusher", null },
                    { 25, 2, 1, 5, "Squat", null },
                    { 26, 2, 1, 5, "Front Squat", null },
                    { 27, 2, 1, 5, "Lunge", null },
                    { 28, 2, 1, 5, "Romanian Deadlift", null },
                    { 29, 2, 1, 5, "Stiff Leg Deadlift", null },
                    { 30, 2, 1, 5, "Calf Raise", null },
                    { 31, 2, 1, 5, "Good Morning", null },
                    { 32, 2, 1, 5, "Zercher Squat", null },
                    { 33, 2, 1, 5, "Hack Squat", null },
                    { 34, 2, 1, 6, "Rollout", null },
                    { 35, 2, 1, 6, "Landmine Rotation", null },
                    { 36, 2, 1, 7, "Hip Thrust", null },
                    { 37, 2, 1, 7, "Glute Bridge", null },
                    { 38, 2, 1, 7, "Sumo Deadlift", null },
                    { 39, 3, 1, 1, "Bench Press", null },
                    { 40, 3, 1, 1, "Incline Bench Press", null },
                    { 41, 3, 1, 1, "Decline Bench Press", null },
                    { 42, 3, 1, 1, "Fly", null },
                    { 43, 3, 1, 1, "Incline Fly", null },
                    { 44, 3, 1, 1, "Pullover", null },
                    { 45, 3, 1, 1, "Squeeze Press", null },
                    { 46, 3, 1, 2, "Row", null },
                    { 47, 3, 1, 2, "Bent Over Row", null },
                    { 48, 3, 1, 2, "Reverse Fly", null },
                    { 49, 3, 1, 2, "Pullover Row", null },
                    { 50, 3, 1, 2, "Renegade Row", null },
                    { 51, 3, 1, 2, "Shrug", null },
                    { 52, 3, 1, 2, "Chest Supported Row", null },
                    { 53, 3, 1, 3, "Shoulder Press", null },
                    { 54, 3, 1, 3, "Arnold Press", null },
                    { 55, 3, 1, 3, "Lateral Raise", null },
                    { 56, 3, 1, 3, "Front Raise", null },
                    { 57, 3, 1, 3, "Rear Delt Fly", null },
                    { 58, 3, 1, 3, "Upright Row", null },
                    { 59, 3, 1, 3, "Y Raise", null },
                    { 60, 3, 1, 3, "Scott Press", null },
                    { 61, 3, 1, 4, "Bicep Curl", null },
                    { 62, 3, 1, 4, "Hammer Curl", null },
                    { 63, 3, 1, 4, "Concentration Curl", null },
                    { 64, 3, 1, 4, "Incline Curl", null },
                    { 65, 3, 1, 4, "Preacher Curl", null },
                    { 66, 3, 1, 4, "Tricep Extension", null },
                    { 67, 3, 1, 4, "Overhead Tricep Extension", null },
                    { 68, 3, 1, 4, "Tricep Kickback", null },
                    { 69, 3, 1, 4, "Zottman Curl", null },
                    { 70, 3, 1, 4, "Wrist Curl", null },
                    { 71, 3, 1, 5, "Squat", null },
                    { 72, 3, 1, 5, "Goblet Squat", null },
                    { 73, 3, 1, 5, "Lunge", null },
                    { 74, 3, 1, 5, "Walking Lunge", null },
                    { 75, 3, 1, 5, "Romanian Deadlift", null },
                    { 76, 3, 1, 5, "Step Up", null },
                    { 77, 3, 1, 5, "Bulgarian Split Squat", null },
                    { 78, 3, 1, 5, "Calf Raise", null },
                    { 79, 3, 1, 5, "Sumo Squat", null },
                    { 80, 3, 1, 6, "Russian Twist", null },
                    { 81, 3, 1, 6, "Side Bend", null },
                    { 82, 3, 1, 6, "Woodchop", null },
                    { 83, 3, 1, 7, "Hip Thrust", null },
                    { 84, 3, 1, 7, "Glute Bridge", null },
                    { 85, 3, 1, 7, "Sumo Deadlift", null },
                    { 86, 3, 1, 7, "Frog Pump", null },
                    { 87, 11, 1, 1, "Chest Fly", null },
                    { 88, 11, 1, 1, "Low Chest Fly", null },
                    { 89, 11, 1, 1, "High Chest Fly", null },
                    { 90, 11, 1, 1, "Crossover", null },
                    { 91, 11, 1, 2, "Seated Row", null },
                    { 92, 11, 1, 2, "Lat Pulldown", null },
                    { 93, 11, 1, 2, "Close Grip Pulldown", null },
                    { 94, 11, 1, 2, "Straight Arm Pulldown", null },
                    { 95, 11, 1, 2, "Face Pull", null },
                    { 96, 11, 1, 2, "Single Arm Row", null },
                    { 97, 11, 1, 2, "Wide Grip Pulldown", null },
                    { 98, 11, 1, 2, "Reverse Grip Pulldown", null },
                    { 99, 11, 1, 3, "Lateral Raise", null },
                    { 100, 11, 1, 3, "Front Raise", null },
                    { 101, 11, 1, 3, "Rear Delt Fly", null },
                    { 102, 11, 1, 3, "Upright Row", null },
                    { 103, 11, 1, 3, "External Rotation", null },
                    { 104, 11, 1, 4, "Bicep Curl", null },
                    { 105, 11, 1, 4, "Hammer Curl", null },
                    { 106, 11, 1, 4, "Overhead Curl", null },
                    { 107, 11, 1, 4, "Tricep Pushdown", null },
                    { 108, 11, 1, 4, "Rope Pushdown", null },
                    { 109, 11, 1, 4, "Overhead Tricep Extension", null },
                    { 110, 11, 1, 4, "Reverse Curl", null },
                    { 111, 11, 1, 4, "Single Arm Curl", null },
                    { 112, 11, 1, 5, "Pull Through", null },
                    { 113, 11, 1, 5, "Leg Extension", null },
                    { 114, 11, 1, 6, "Crunch", null },
                    { 115, 11, 1, 6, "Woodchop", null },
                    { 116, 11, 1, 6, "Pallof Press", null },
                    { 117, 11, 1, 6, "Reverse Crunch", null },
                    { 118, 11, 1, 7, "Kickback", null },
                    { 119, 11, 1, 7, "Hip Abduction", null },
                    { 120, 11, 1, 7, "Hip Adduction", null },
                    { 121, 4, 1, 1, "Chest Press", null },
                    { 122, 4, 1, 1, "Incline Chest Press", null },
                    { 123, 4, 1, 1, "Pec Deck Fly", null },
                    { 124, 4, 1, 2, "Seated Row", null },
                    { 125, 4, 1, 2, "Lat Pulldown", null },
                    { 126, 4, 1, 2, "Assisted Pull Up", null },
                    { 127, 4, 1, 2, "Reverse Fly", null },
                    { 128, 4, 1, 2, "T-Bar Row", null },
                    { 129, 4, 1, 3, "Shoulder Press", null },
                    { 130, 4, 1, 3, "Lateral Raise", null },
                    { 131, 4, 1, 3, "Rear Delt Fly", null },
                    { 132, 4, 1, 4, "Bicep Curl", null },
                    { 133, 4, 1, 4, "Tricep Extension", null },
                    { 134, 4, 1, 4, "Preacher Curl", null },
                    { 135, 4, 1, 4, "Tricep Dip", null },
                    { 136, 4, 1, 5, "Leg Press", null },
                    { 137, 4, 1, 5, "Hack Squat", null },
                    { 138, 4, 1, 5, "Leg Extension", null },
                    { 139, 4, 1, 5, "Leg Curl", null },
                    { 140, 4, 1, 5, "Seated Leg Curl", null },
                    { 141, 4, 1, 5, "Standing Calf Raise", null },
                    { 142, 4, 1, 5, "Seated Calf Raise", null },
                    { 143, 4, 1, 5, "Smith Squat", null },
                    { 144, 4, 1, 5, "Pendulum Squat", null },
                    { 145, 4, 1, 5, "V Squat", null },
                    { 146, 4, 1, 6, "Ab Crunch", null },
                    { 147, 4, 1, 6, "Torso Rotation", null },
                    { 148, 4, 1, 7, "Hip Thrust", null },
                    { 149, 4, 1, 7, "Glute Kickback", null },
                    { 150, 4, 1, 7, "Hip Abduction", null },
                    { 151, 4, 1, 7, "Hip Adduction", null },
                    { 152, 4, 1, 8, "Smith Machine Squat", null },
                    { 153, 4, 1, 8, "Smith Machine Bench Press", null },
                    { 154, 6, 2, 1, "Push Up", null },
                    { 155, 6, 2, 1, "Diamond Push Up", null },
                    { 156, 6, 2, 1, "Wide Push Up", null },
                    { 157, 6, 2, 1, "Decline Push Up", null },
                    { 158, 6, 2, 1, "Dip", null },
                    { 159, 6, 2, 2, "Pull Up", null },
                    { 160, 6, 2, 2, "Chin Up", null },
                    { 161, 6, 2, 2, "Wide Grip Pull Up", null },
                    { 162, 6, 2, 2, "Inverted Row", null },
                    { 163, 6, 2, 2, "Neutral Grip Pull Up", null },
                    { 164, 6, 2, 3, "Pike Push Up", null },
                    { 165, 6, 2, 3, "Handstand Push Up", null },
                    { 166, 6, 2, 4, "Tricep Dip", null },
                    { 167, 6, 2, 4, "Bench Dip", null },
                    { 168, 6, 2, 4, "Close Grip Push Up", null },
                    { 169, 6, 2, 5, "Squat", null },
                    { 170, 6, 2, 5, "Lunge", null },
                    { 171, 6, 2, 5, "Pistol Squat", null },
                    { 172, 6, 2, 5, "Jump Squat", null },
                    { 173, 6, 2, 5, "Wall Sit", null },
                    { 174, 6, 2, 5, "Calf Raise", null },
                    { 175, 6, 2, 5, "Box Jump", null },
                    { 176, 6, 2, 6, "Crunch", null },
                    { 177, 6, 2, 6, "Sit Up", null },
                    { 178, 6, 2, 6, "Plank", null },
                    { 179, 6, 2, 6, "Side Plank", null },
                    { 180, 6, 2, 6, "Leg Raise", null },
                    { 181, 6, 2, 6, "Hanging Leg Raise", null },
                    { 182, 6, 2, 6, "Hanging Knee Raise", null },
                    { 183, 6, 2, 6, "Mountain Climber", null },
                    { 184, 6, 2, 6, "Bicycle Crunch", null },
                    { 185, 6, 2, 6, "Flutter Kicks", null },
                    { 186, 6, 2, 6, "Russian Twist", null },
                    { 187, 6, 2, 6, "V Up", null },
                    { 188, 6, 2, 6, "Dead Bug", null },
                    { 189, 6, 2, 6, "Ab Wheel Rollout", null },
                    { 190, 6, 2, 7, "Glute Bridge", null },
                    { 191, 6, 2, 7, "Single Leg Glute Bridge", null },
                    { 192, 6, 2, 7, "Donkey Kick", null },
                    { 193, 6, 2, 7, "Fire Hydrant", null },
                    { 194, 6, 2, 8, "Burpee", null },
                    { 195, 6, 2, 8, "Bear Crawl", null },
                    { 196, 8, 1, 2, "Pull Up", null },
                    { 197, 8, 1, 2, "Chin Up", null },
                    { 198, 8, 1, 1, "Dip", null },
                    { 199, 8, 1, 4, "Tricep Dip", null },
                    { 200, 8, 1, 5, "Pistol Squat", null },
                    { 201, 10, 1, 8, "Clean", null },
                    { 202, 10, 1, 8, "Clean and Jerk", null },
                    { 203, 10, 1, 8, "Snatch", null },
                    { 204, 10, 1, 8, "Power Clean", null },
                    { 205, 10, 1, 8, "Hang Clean", null },
                    { 206, 10, 1, 3, "Push Press", null },
                    { 207, 10, 1, 2, "Clean Pull", null },
                    { 208, 10, 1, 2, "Snatch Pull", null },
                    { 209, 10, 1, 8, "Thruster", null },
                    { 210, 10, 1, 8, "Overhead Squat", null },
                    { 211, 1, 3, 8, "Running", null },
                    { 212, 1, 3, 8, "Treadmill Running", null },
                    { 213, 1, 3, 5, "Cycling", null },
                    { 214, 1, 3, 5, "Stationary Bike", null },
                    { 215, 1, 3, 8, "Elliptical", null },
                    { 216, 1, 3, 8, "Rowing Machine", null },
                    { 217, 1, 3, 5, "Stair Climber", null },
                    { 218, 1, 3, 8, "Jump Rope", null },
                    { 219, 1, 3, 8, "Swimming", null },
                    { 220, 1, 3, 8, "Walking", null },
                    { 221, 1, 3, 5, "Incline Walking", null },
                    { 222, 1, 3, 8, "Battle Ropes", null },
                    { 223, 1, 3, 8, "Assault Bike", null },
                    { 224, 1, 3, 8, "Ski Erg", null },
                    { 225, 1, 3, 5, "Sprinting", null },
                    { 226, 7, 3, 8, "Yoga", null },
                    { 227, 7, 3, 6, "Pilates", null },
                    { 228, 7, 3, 8, "Foam Rolling", null },
                    { 229, 7, 3, 8, "Sauna", null },
                    { 230, 7, 3, 5, "Hiking", null },
                    { 231, 7, 3, 8, "Active Recovery Walk", null },
                    { 232, 9, 4, 5, "Hamstring Stretch", null },
                    { 233, 9, 4, 5, "Quad Stretch", null },
                    { 234, 9, 4, 5, "Hip Flexor Stretch", null },
                    { 235, 9, 4, 1, "Chest Stretch", null },
                    { 236, 9, 4, 3, "Shoulder Stretch", null },
                    { 237, 9, 4, 4, "Tricep Stretch", null },
                    { 238, 9, 4, 2, "Lat Stretch", null },
                    { 239, 9, 4, 5, "Calf Stretch", null },
                    { 240, 9, 4, 7, "Glute Stretch", null },
                    { 241, 9, 4, 3, "Neck Stretch", null },
                    { 242, 9, 4, 6, "Cat Cow Stretch", null },
                    { 243, 9, 4, 2, "Child's Pose", null },
                    { 244, 9, 4, 7, "Pigeon Stretch", null },
                    { 245, 9, 4, 5, "Seated Forward Fold", null },
                    { 246, 9, 4, 6, "Spinal Twist", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name_ExerciseCategoryId",
                table: "Exercises",
                columns: new[] { "Name", "ExerciseCategoryId" },
                unique: true,
                filter: "\"UserId\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Exercises_Name_ExerciseCategoryId",
                table: "Exercises");

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 246);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name",
                table: "Exercises",
                column: "Name",
                unique: true,
                filter: "\"UserId\" IS NULL");
        }
    }
}
