public class Student
{
    public int Id;
    public string Name;
    public int BirthYear;
    public string Address;
    public double Grade;
    public string ImageUrl;

    public Student(int id, string name, int birthYear, string address, double grade, string imageUrl)
    {
        Id = id;
        Name = name;
        BirthYear = birthYear;
        Address = address;
        Grade = grade;
        ImageUrl = imageUrl;
    }
}
