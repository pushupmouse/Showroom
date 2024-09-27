using Firebase.Firestore;

[FirestoreData]
public class Student
{
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public string Name {  get; set; }
    [FirestoreProperty] public int BirthYear { get; set; }
    [FirestoreProperty] public string Address { get; set; }
    [FirestoreProperty] public double Grade { get; set; }
    [FirestoreProperty] public string ImageName { get; set; }

    public Student(string name, int birthYear, string address, double grade, string imageName)
    {
        Name = name;
        BirthYear = birthYear;
        Address = address;
        Grade = grade;
        ImageName = imageName;
    }

    public Student() { }
}
