using Firebase.Firestore;

[FirestoreData]
public class Student
{
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public string Name {  get; set; }
    [FirestoreProperty] public int BirthYear { get; set; }
    [FirestoreProperty] public string Address { get; set; }
    [FirestoreProperty] public double Grade { get; set; }
    [FirestoreProperty] public string ImageUrl { get; set; }

    public Student(string name, int birthYear, string address, double grade, string imageUrl)
    {
        Name = name;
        BirthYear = birthYear;
        Address = address;
        Grade = grade;
        ImageUrl = imageUrl;
    }

    public Student() { }
}
