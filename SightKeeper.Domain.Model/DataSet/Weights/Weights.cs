﻿using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Weights
{
    public byte[] Data { get; private set; }
    public DateTime TrainedDate { get; private set; }
    public ModelConfig Model { get; private set; }
    public WeightsLibrary? Library { get; internal set; }
    public int Epoch { get; private set; }
    public float BoundingLoss { get; private set; }
    public float ClassificationLoss { get; private set; }
    public IReadOnlyCollection<Asset> Assets { get; private set; }
    
    public Weights(byte[] data, DateTime trainedDate, ModelConfig model, WeightsLibrary? library, int epoch, float boundingLoss, float classificationLoss, IReadOnlyCollection<Asset> assets)
    {
        Data = data;
        TrainedDate = trainedDate;
        Model = model;
        Library = library;
        Epoch = epoch;
        BoundingLoss = boundingLoss;
        ClassificationLoss = classificationLoss;
        Assets = assets;
    }

    private Weights()
    {
        Data = null!;
        Model = null!;
        Library = null!;
        Assets = null!;
    }
}