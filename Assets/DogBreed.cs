using System;

[Serializable]
public class DogBreed
{
    public int id;
    public string name;
    public string temperament;
    public string life_span;
    public string origin;
    public BreedImage image;
}

[Serializable]
public class BreedImage
{
    public string url;
}