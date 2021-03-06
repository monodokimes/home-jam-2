﻿using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System.Linq;

public class DogFactory : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnTargetTransform;
    [SerializeField]
    private float _spawnDistanceFromTarget;
    [SerializeField]
    private GameObject _dog;
    [SerializeField]
    private GameObject[] _dogGraphics;
    [SerializeField]
    private int _minSize = 10;
    [SerializeField]
    private int _maxSize = 50;
    [SerializeField]
    private double _sizeLambda = 25;
    [SerializeField]
    private float _dogScale = 0.05f;
    [SerializeField]
    private int _dogQuotes = 5;

    [SerializeField]
    private TextAsset _nameFile;
    private string[] _nameList;
    private int _nameCount;

    [SerializeField]
    private TextAsset _likeFile;
    private string[] _likeList;
    private int _likeCount;
    [SerializeField]
    private int _maxLikeLength = 50;
    [SerializeField]
    private float _doubleLikeChance = .4f;

    [SerializeField]
    private TextAsset _quoteFile;
    private string[] _quotes;

    public GameObject[] DogGraphics { get { return _dogGraphics; } }

    void Awake()
    {
        _nameList = _nameFile.text.Split('\n');
        _nameCount = _nameList.Length;

        _likeList = _likeFile.text.Split('\n');
        _likeCount = _likeList.Length;

        _quotes = _quoteFile.text
            .Split('\n');
    }

    public Dog GetNewDog()
    {
        return SpawnDog(GetNewDogProfile());
    }

    public Dog SpawnDog(DogProfile profile)
    {
        var dog = Instantiate(_dog)
            .GetComponent<Dog>();
        dog.Profile = profile;

        var offset = new Vector3
        {
            x = _spawnTargetTransform.position.x + (UnityEngine.Random.value > 0.5f ? -1 : 1) * _spawnDistanceFromTarget
        };
        dog.transform.Translate(offset);

        var graphics = Instantiate(_dogGraphics[profile.Index], dog.graphics.transform);
        graphics.transform.localScale = Vector3.one * profile.Size;

        return dog;
    }

    public DogProfile GetNewDogProfile()
    {
        var size = GetRandomSize();
        var profile = new DogProfile
        {
            Name = GetRandomName(),
            Size = MultiplierFromSize(size),
            Like = GetLikeString(),
            Pitch = PitchFromSize(size),
            Quotes = GetRandomQuotes(_dogQuotes),
            Index = GetRandomIndex()
        };
        return profile;
    }

    private float GetRandomSize()
    {
        // first, do a rough poisson calculation
        double p = 1.0, L = Math.Exp(-_sizeLambda);
        int k = 0;
        do
        {
            k++;
            p *= UnityEngine.Random.value;
        }
        while (p > L);
        k--;
        // now we've got our poisson, let's clamp it
        return Mathf.Clamp((float)k, _minSize, _maxSize);
    }

    private int GetRandomIndex()
    {
        int max = _dogGraphics.Length;
        if (UnityEngine.Random.Range(0f, 1f) < 0.01337f)
            return max - 1;
        return UnityEngine.Random.Range(0, max - 1);
    }

    private float MultiplierFromSize(float size)
    {
        return size * _dogScale;
    }

    private float PitchFromSize(float size)
    {
        return Mathf.Clamp(2.5f - 2 * (size / _maxSize), .5f, 1.5f);
    }

    private string GetRandomName()
    {
        string name = _nameList[UnityEngine.Random.Range(0, _nameCount)];
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower().Trim());
    }

    private string GetRandomLike()
    {
        string like = _likeList[UnityEngine.Random.Range(0, _likeCount)];
        return like.ToLower().Trim();
    }

    private string[] GetRandomQuotes(int count)
    {
        var result = new string[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = GetRandomQuote();
        }

        return result;
    }

    private string GetRandomQuote()
    {
        return _quotes[UnityEngine.Random.Range(0, _quotes.Length)]
            .ToLower()
            .Trim();
    }

    private string GetLikeString()
    {
        if (UnityEngine.Random.Range(0f, 1f) < _doubleLikeChance)
        {
            string likeString;
            while (true)
            {
                likeString = string.Format("{0} and {1}", GetRandomLike(), GetRandomLike());
                if (likeString.Length < _maxLikeLength)
                {
                    return likeString;
                }
            }
        }
        else
        {
            return GetRandomLike();
        }
    }
}
