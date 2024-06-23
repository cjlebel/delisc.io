using System;
using System.Collections;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using Deliscio.Modules.Links.Domain.LinkTags;
using Xunit;

namespace Deliscio.Tests.Unit.Modules.Domain.LinkTags;

public class LinkTagCollectionTests
{
    private LinkTagCollection _testClass;
    private IFixture _fixture;
    private IEnumerable<LinkTag> _tags;

    public LinkTagCollectionTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _tags = new[]
        {
            LinkTag.Create("Tag1", 100, 0.50M),
            LinkTag.Create("Tag2", 40, 0.20M),
            LinkTag.Create("Tag3", 30, 0.15M),
            LinkTag.Create("Tag4", 20, 0.10M),
            LinkTag.Create("Tag5", 10, 0.05M)
        };

        _testClass = new LinkTagCollection(_tags);
    }

    [Fact]
    public void Can_Construct()
    {
        // Act
        var instance = new LinkTagCollection(_tags);

        // Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void Can_Construct_WithNull_Tags()
    {
        var collection = new LinkTagCollection(default(IEnumerable<LinkTag>));

        Assert.NotNull(collection);
    }

    [Fact]
    public void Implements_IEnumerable_LinkTag()
    {
        // Arrange
        var linkTags = new LinkTag[]
        {
            _fixture.Create<LinkTag>(), _fixture.Create<LinkTag>(), _fixture.Create<LinkTag>(),
            _fixture.Create<LinkTag>(), _fixture.Create<LinkTag>(),
        };

        var linkTagCollections = new LinkTagCollection(linkTags);
        
        int expectedCount = linkTags.Length;
        int actualCount = 0;

        // Act
        using (var enumerator = linkTagCollections.GetEnumerator())
        {
            Assert.NotNull(enumerator);
            while (enumerator.MoveNext())
            {
                actualCount++;
                Assert.IsType<LinkTag>(enumerator.Current);
            }
        }

        // Assert
        Assert.Equal(expectedCount, actualCount);
    }

    [Fact]
    public void CanCall_Empty()
    {
        // Act
        // Creates a new instance of LinkTagCollection with no tags (same usage as doing Array.Empty<T>())
        var result = LinkTagCollection.Empty();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void CanCall_AddWithLinkTag()
    {
        // Arrange
        var tag = _fixture.Create<LinkTag>();

        // Act
        _testClass.Add(tag);

        // Assert
        var result = _testClass.Contains(tag);
    }

    [Fact]
    public void CannotCall_AddWithLinkTag_WithNull_Tag()
    {
        var previousCount = _testClass.Count();

        _testClass.Add(default(LinkTag));

        Assert.Equal(previousCount, _testClass.Count());
    }

    [Fact]
    public void CanCall_Add_With_IEnumerable_Of_LinkTag()
    {
        // Arrange
        var newTags = new[] { LinkTag.New("Tag1"), LinkTag.New("Tag2"), LinkTag.New("Tag4"), LinkTag.New("Tag5"), LinkTag.New("Tag6") };

        var previousTags = _testClass.Tags;

        var expectedTagNames = previousTags
            .Select(t1 => t1.Name)
            .Union(newTags.Select(t2 => t2.Name))
            .ToArray();

        // Act
        _testClass.Add(newTags);

        // Assert
        Assert.NotNull(_testClass);
        Assert.Equal(expectedTagNames.Length, _testClass.Count());

        // Verify that all expected tags are present in the collection
        foreach (var expectedTagName in expectedTagNames)
        {
            var existingTag = _testClass.Tags.SingleOrDefault(t => t.Name.Equals(expectedTagName.ToLower()));

            Assert.NotNull(existingTag);

            // TODO: Check counts and weights
            // New tags should have Count = 1.
            // Previous tags that are also in the new tags should have their Count incremented by 1.
            // Weights should be recalculated.
        }
    }

    [Fact]
    public void CannotCall_Add_With_IEnumerableOf_LinkTag_WithNull_Tags()
    {
        var previousCount = _testClass.Count();

        _testClass.Add(default(IEnumerable<LinkTag>));

        Assert.Equal(previousCount, _testClass.Count());
    }

    [Fact]
    public void CanCall_Delete_With_String()
    {
        // Arrange
        var previousCount = _testClass.Count();
        var tagToDelete = _testClass.Tags.First();

        // Act
        _testClass.Delete(tagToDelete.Name.ToUpper());

        // Assert
        Assert.True(_testClass.Count() == previousCount - 1);
        Assert.Null(_testClass.Tags.SingleOrDefault(t => t.Name.Equals(tagToDelete.Name)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CannotCall_Delete_With_String_WithInvalid_Tag(string value)
    {
        var previousCount = _testClass.Count();

        // Nothing will happen if an invalid string is passed in
        _testClass.Delete(value);

        Assert.Equal(previousCount, _testClass.Count());
    }

    [Fact]
    public void CanCall_DeleteWithLinkTag()
    {
        // Arrange
        var previousCount = _testClass.Count();
        var tagToDelete = _testClass.Tags.First();

        // Act
        _testClass.Delete(tagToDelete);

        // Assert
        Assert.True(_testClass.Count() == previousCount - 1);
        Assert.Null(_testClass.Tags.SingleOrDefault(t => t.Name.Equals(tagToDelete.Name)));
    }

    [Fact]
    public void CanCall_Delete_With_LinkTag_WithNull_Tag()
    {
        var previousCount = _testClass.Count();

        // Should not throw exception
        _testClass.Delete(default(LinkTag));

        Assert.Equal(previousCount, _testClass.Count());
    }


    //[Fact]
    //public void CanCall_Remove_With_LinkTag()
    //{
    //    // Arrange
    //    var priorTag = _testClass.Tags.First();
    //    var priorCount = priorTag.Count;

    //    // Act
    //    _testClass.Remove(priorTag);

    //    var actualTag = _testClass.Tags.SingleOrDefault(t => t.Name.Equals(priorTag.Name, StringComparison.InvariantCultureIgnoreCase));
    //    var actualCount = actualTag?.Count;

    //    // Assert
    //    Assert.Equal(priorCount - 1, actualCount);
    //}

    //[Fact]
    //public void CanCall_Remove_With_LinkTag_WithNull_Tag()
    //{
    //    var priorTag = _testClass.Count();

    //    // Should not throw exception
    //    _testClass.Remove(default(LinkTag));

    //    Assert.Equal(priorTag, _testClass.Count());
    //}

    //[Fact]
    //public void CanCall_Remove_With_ArrayOf_LinkTag()
    //{
    //    // Arrange
    //    var priorTags = new[] { _testClass.First(), _testClass.Last() };
    //    var priorTagCounts = priorTags.Select(t => t.Count).ToArray();

    //    // Act
    //    _testClass.Remove(priorTags);

    //    // Assert
    //    for(var i = 0; i < priorTags.Length; i++)
    //    {
    //        var priorTag = priorTags[i];
    //        var priorCount = priorTagCounts[i];

    //        if(priorTag.Count > 1)
    //        {
    //            var actualTag = _testClass.Tags.SingleOrDefault(t => t.Name.Equals(priorTag.Name, StringComparison.InvariantCultureIgnoreCase));

    //            Assert.NotNull(actualTag);
    //            Assert.Equal(priorCount - 1, actualTag.Count);
    //        }
    //        else
    //        {
    //            Assert.Null(_testClass.Tags.SingleOrDefault(t => t.Name.Equals(priorTag.Name, StringComparison.InvariantCultureIgnoreCase)));
    //        }
    //    }
    //}

    //[Fact]
    //public void CanCall_Remove_With_ArrayOf_LinkTag_WithNull_Tags()
    //{
    //    // If null tags are passed in, nothing will happen (no exceptions)
    //    _testClass.Remove(default(LinkTag[]));
    //}

    [Fact]
    public void CanCall_Clear()
    {
        // Act
        var tagCollection = _testClass;

        tagCollection.Clear();

        // Assert
        Assert.Empty(tagCollection);
    }

    [Fact]
    public void CanCall_IsEmpty()
    {
        // Act
        var tagCollection = _testClass;

        // Assert
        Assert.False(tagCollection.IsEmpty());

        tagCollection.Clear();

        Assert.True(tagCollection.IsEmpty());
    }

    //[Fact]
    //public void CanCall_GetEnumerator_With_No_Parameters()
    //{
    //    // Act
    //    var result = _testClass.GetEnumerator();

    //    // Assert
    //    throw new NotImplementedException("Create or modify test");
    //}

    [Fact]
    public void CanCall_ToString()
    {
        // Act
        var result = _testClass.ToString();

        // Assert
        Assert.True(!string.IsNullOrWhiteSpace(result));

        Assert.True(result.Split(',').Length > 0);
    }

    [Fact]
    public void CanCall_GetEnumerator_For_IEnumerable_With_No_Parameters()
    {
        // Act
        var result = ((IEnumerable)_testClass).GetEnumerator();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Check_Sum_Of_All_Weights_Should_Be_1()
    {
        var tags = _testClass.Tags;

        Assert.NotNull(tags);

        // Get the total count of all tags
        var totalCount = tags.Sum(t => t.Count);

        Assert.Equal(1M, tags.Sum(t => t.Weight));
    }
}