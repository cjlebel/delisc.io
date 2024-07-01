using AutoFixture;
using AutoFixture.AutoMoq;
using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using Deliscio.Modules.Links.Domain.Links;
using Deliscio.Modules.Links.Domain.LinkTags;
using Deliscio.Modules.Links.Infrastructure.Data.Entities;
using MongoDB.Bson;

namespace Deliscio.Tests.Unit.Modules.Links.Domain.Links;

public class LinkTests
{
    private readonly Link _testClass;
    private readonly IFixture _fixture;

    protected CreatedById CreatedByUserIdValueObject;
    protected DeletedById DeletedByUserIdValueObject;
    protected UpdatedById UpdatedByUserIdValueObject;

    public LinkTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _testClass = Link.Map(GetInitEntity());

        CreatedByUserIdValueObject = CreatedById.Create(ObjectId.GenerateNewId());
        DeletedByUserIdValueObject = DeletedById.Create(ObjectId.GenerateNewId());
        UpdatedByUserIdValueObject = UpdatedById.Create(ObjectId.GenerateNewId());
    }

    [Fact]
    public void CanCall_New()
    {
        // Arrange
        var title = _fixture.Create<LinkTitle>();
        var description = _fixture.Create<LinkDescription>();
        var url = new LinkUrl("https://www.delisc.io/some/url");
        var domain = new LinkDomain("delisc.io");
        var imageUrl = _fixture.Create<LinkImageUrl>();
        var createdByUserId = CreatedById.Create(ObjectId.GenerateNewId());
        var dateCreated = _fixture.Create<DateTimeOffset>();
        var tags = _fixture.Create<LinkTag[]>();
        var keywords = _fixture.Create<string>();

        // Act
        var actual = Link.New(title, description, url, imageUrl, createdByUserId, dateCreated, tags, keywords);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<Link>(actual);
        Assert.Equal(title.Value, actual.Title.Value);
        Assert.Equal(description.Value, actual.Description.Value);
        Assert.Equal(url.Value, actual.Url.Value);
        Assert.Equal(domain.Value, actual.Domain.Value);
        Assert.Equal(imageUrl.Value, actual.ImageUrl.Value);
        Assert.Equal(createdByUserId.Value, actual.CreatedById.Value);
        Assert.Equal(dateCreated, actual.DateCreated);
        Assert.Equal(tags.Length, actual.TagsCollection.Count());
        Assert.Equal(keywords, string.Join(',', actual.Keywords.ToList()));
    }

    [Fact]
    public void CannotCall_New_WithNull_Title()
    {
        Assert.Throws<ArgumentNullException>(() => Link.New(default, _fixture.Create<LinkDescription>(), _fixture.Create<LinkUrl>(), _fixture.Create<LinkImageUrl>(), _testClass.CreatedById, _fixture.Create<DateTimeOffset>(), _fixture.Create<LinkTag[]>(), _fixture.Create<string>()));
    }


    [Fact]
    public void CannotCall_New_WithNull_Url()
    {
        Assert.Throws<ArgumentNullException>(() => Link.New(_fixture.Create<LinkTitle>(), _fixture.Create<LinkDescription>(), default, _fixture.Create<LinkImageUrl>(), _testClass.CreatedById, _fixture.Create<DateTimeOffset>(), _fixture.Create<LinkTag[]>(), _fixture.Create<string>()));
    }


    [Fact]
    public void CannotCall_New_WithNull_CreatedBy()
    {
        Assert.Throws<ArgumentNullException>(() => Link.New(_fixture.Create<LinkTitle>(), _fixture.Create<LinkDescription>(), _fixture.Create<LinkUrl>(), _fixture.Create<LinkImageUrl>(), default, _fixture.Create<DateTimeOffset>(), _fixture.Create<LinkTag[]>(), _fixture.Create<string>()));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CanCall_New_WithEmpty_Keywords(string? value)
    {
        var actual = Link.New(
            _fixture.Create<LinkTitle>(),
            _fixture.Create<LinkDescription>(),
            _fixture.Create<LinkUrl>(),
            _fixture.Create<LinkImageUrl>(),
            CreatedByUserIdValueObject,
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<LinkTag[]>(),
            value);

        Assert.NotNull(actual);
        Assert.Equal([], actual.Keywords);
    }

    [Fact]
    public void CanCall_New_With_Keywords()
    {
        var expectedKeywords = "keyword1, keyword2, keyword3";

        var actual = Link.New(
            _fixture.Create<LinkTitle>(),
            _fixture.Create<LinkDescription>(),
            _fixture.Create<LinkUrl>(),
            _fixture.Create<LinkImageUrl>(),
            CreatedByUserIdValueObject,
            _fixture.Create<DateTimeOffset>(),
            _fixture.Create<LinkTag[]>(),
            expectedKeywords);

        Assert.NotNull(actual);
        Assert.Equal(3, actual.Keywords.Count);
    }

    [Fact]
    public void New_PerformsMapping()
    {
        // Arrange
        var title = _fixture.Create<LinkTitle>();
        var description = _fixture.Create<LinkDescription>();
        var url = _fixture.Create<LinkUrl>();
        var imageUrl = _fixture.Create<LinkImageUrl>();
        var tags = _fixture.Create<LinkTag[]>();
        var keywords = "keyword1,keyword2,keyword3,keyword4";
        var likesCount = _fixture.Create<int>();
        var savesCount = _fixture.Create<int>();
        var isActive = _fixture.Create<bool>();
        var isFlagged = _fixture.Create<bool>();
        var isDeleted = _fixture.Create<bool>();
        var createdBy = CreatedByUserIdValueObject;
        var dateCreated = _fixture.Create<DateTimeOffset>();
        var updatedBy = UpdatedByUserIdValueObject;
        var dateUpdated = _fixture.Create<DateTimeOffset>();
        var deletedBy = DeletedByUserIdValueObject;
        var dateDeleted = _fixture.Create<DateTimeOffset>();

        // Act
        var result = Link.New(title, description, url, imageUrl, createdBy, dateCreated, tags, keywords);

        // Assert
        Assert.Same(title, result.Title);
        Assert.Same(description, result.Description);
        Assert.Same(url, result.Url);
        Assert.Same(imageUrl, result.ImageUrl);
        Assert.Equal(dateCreated, result.DateCreated);

        Assert.NotNull(result.TagsCollection);
        Assert.Equal(tags.Length, result.TagsCollection.Count());
        Assert.Equal(keywords.Split(','), result.Keywords.ToArray());

        Assert.Equal(true, result.IsActive);
        Assert.Equal(false, result.IsFlagged);
        Assert.Equal(false, result.IsDeleted);

        Assert.Equal(0, result.LikesCount);
        Assert.Equal(0, result.SavesCount);

        Assert.Equal(createdBy.Value, result.CreatedById.Value);
        Assert.Equal(dateCreated, result.DateCreated);

        Assert.Equal(ObjectId.Empty, result.UpdatedById.Value);
        Assert.Null(result.DateUpdated);

        Assert.Equal(ObjectId.Empty, result.DeletedById.Value);
        Assert.Null(result.DateDeleted);
    }

    [Fact]
    public void Map_PerformsMapping()
    {
        // Arrange
        // Arrange
        var id = ObjectId.GenerateNewId();
        var title = _fixture.Create<LinkTitle>();
        var description = _fixture.Create<LinkDescription>();
        var url = "https://www.delisc.io/some/random/url";
        var imageUrl = _fixture.Create<LinkImageUrl>();
        var tags = _fixture.Create<LinkTag[]>();
        var keywords = "keyword1,keyword2,keyword3,keyword4";
        var likesCount = _fixture.Create<int>();
        var savesCount = _fixture.Create<int>();
        var isActive = _fixture.Create<bool>();
        var isFlagged = _fixture.Create<bool>();
        var isDeleted = _fixture.Create<bool>();
        var createdBy = CreatedByUserIdValueObject;
        var dateCreated = _fixture.Create<DateTimeOffset>();
        var updatedBy = UpdatedByUserIdValueObject;
        var dateUpdated = _fixture.Create<DateTimeOffset>();
        var deletedBy = DeletedByUserIdValueObject;
        var dateDeleted = _fixture.Create<DateTimeOffset>();

        var entity = new LinkEntity()
        {
            Id = id,
            Title = title.Value,
            Description = description.Value,
            Url = url,
            Domain = "delisc.io",
            ImageUrl = imageUrl.Value,
            Keywords = keywords.Split(','),
            LikesCount = likesCount,
            SavesCount = savesCount,
            IsActive = isActive,
            IsFlagged = isFlagged,
            IsDeleted = isDeleted,
            CreatedById = createdBy.Value,
            DateCreated = dateCreated,
            UpdatedById = updatedBy.Value,
            DateUpdated = dateUpdated,
            DeletedById = deletedBy.Value,
            DateDeleted = dateDeleted,
            Tags = tags.Select(t => new LinkTagEntity(t.Name, t.Count, t.Weight)).ToArray(),
        };

        // Act
        var result = Link.Map(entity);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Link>(result);
        Assert.Equal(entity.Title, result.Title.Value);
        Assert.Equal(entity.Description, result.Description.Value);
        Assert.Equal(entity.Domain, result.Domain.Value);
        Assert.Equal(entity.ImageUrl, result.ImageUrl.Value);
        Assert.Equal(entity.Keywords, result.Keywords.ToArray());
        Assert.Equal(entity.IsActive, result.IsActive);
        Assert.Equal(entity.IsFlagged, result.IsFlagged);
        Assert.Equal(entity.SavesCount, result.SavesCount);
        Assert.Equal(entity.LikesCount, result.LikesCount);
        Assert.Equal(entity.Url, result.Url.Value);
        Assert.Equal(entity.IsDeleted, result.IsDeleted);
        Assert.Equal(entity.DateDeleted, result.DateDeleted);
        Assert.Equal(entity.DeletedById, result.DeletedById.Value);
        Assert.Equal(entity.CreatedById, result.CreatedById.Value);
        Assert.Equal(entity.DateCreated, result.DateCreated);
        Assert.Equal(entity.UpdatedById, result.UpdatedById.Value);
        Assert.Equal(entity.DateUpdated, result.DateUpdated);
        Assert.Equal(entity.Tags.Length, result.TagsCollection.Count());
    }

    [Fact]
    public void CannotCall_Map_WithNull_Entity()
    {
        var actual = Link.Map(default);

        Assert.Null(actual);
    }


    [Fact]
    public void CanCall_Delete()
    {
        // Act
        _testClass.Delete(DeletedByUserIdValueObject);

        // Assert
        Assert.True(_testClass.IsDeleted);
        Assert.Equal(DeletedByUserIdValueObject.Value.ToString(), _testClass.DeletedById.Value.ToString());
        Assert.True(_testClass.DateDeleted >= DateTimeOffset.Now.AddMinutes(-5));

        // Deleted and Updated user Ids should be the same
        Assert.Equal(DeletedByUserIdValueObject.Value.ToString(), _testClass.UpdatedById.Value.ToString());

        // Date deleted and updated should be the same
        Assert.Equal(_testClass.DateDeleted, _testClass.DateUpdated);

        Assert.False(_testClass.IsActive);
    }

    [Fact]
    public void CannotCall_Delete_WithNull_DeletedById()
    {
        Assert.Throws<ArgumentNullException>(() => _testClass.Delete(default));
    }

    [Fact]
    public void CanCall_UnDelete()
    {
        if (!_testClass.IsDeleted)
        {
            _testClass.Delete(DeletedByUserIdValueObject);
        }

        var previousUpdateDate = _testClass.DateUpdated;

        // Act
        _testClass.UnDelete(UpdatedByUserIdValueObject);

        // Assert
        Assert.False(_testClass.IsDeleted);
        Assert.Equal(ObjectId.Empty, _testClass.DeletedById.Value);
        Assert.Null(_testClass.DateDeleted);

        Assert.True(_testClass.DateUpdated > previousUpdateDate);
        Assert.Equal(UpdatedByUserIdValueObject.Value, _testClass.UpdatedById.Value);
    }

    [Fact]
    public void CannotCall_UnDelete_WithNull_UpdatedById()
    {
        Assert.Throws<ArgumentNullException>(() => _testClass.UnDelete(default));
    }

    [Fact]
    public void CanCall_Edit()
    {
        // Arrange
        var title = _fixture.Create<LinkTitle>();
        var description = _fixture.Create<LinkDescription>();

        var newTags = new[] { LinkTag.New("Tag1"), LinkTag.New("Tag2"), LinkTag.New("Tag4"), LinkTag.New("Tag5"), LinkTag.New("Tag6") };

        var previousTags = _testClass.TagsCollection;

        // Act
        _testClass.Edit(title, description, newTags, UpdatedByUserIdValueObject);

        // Assert
        Assert.Equal(title, _testClass.Title);
        Assert.Equal(description, _testClass.Description);
        Assert.Equal(UpdatedByUserIdValueObject.Value, _testClass.UpdatedById.Value);
        Assert.True(_testClass.DateUpdated >= DateTimeOffset.Now.AddMinutes(-5));

        Assert.Equal(newTags.Length, _testClass.TagsCollection.Count());

        foreach (var newTag in newTags)
        {
            Assert.NotNull(_testClass.TagsCollection.Select(t => t.Name.Equals(newTag.Name.ToLower())));
        }
    }

    /// <summary>
    /// If the Link is marked as IsDeleted, then the Edit method should not be able to be called.
    /// </summary>
    [Fact]
    public void CanNotCall_Edit_When_IsDeleted_Is_True()
    {
        // Set IsDeleted to true
        _testClass.Delete(DeletedByUserIdValueObject);

        // Get the existing values
        var previousTitle = _testClass.Title;
        var previousDescription = _testClass.Description;

        var previousTags = _testClass.TagsCollection;
        var dateUpdatedDeleted = _testClass.DateDeleted;

        // Arrange
        var title = _fixture.Create<LinkTitle>();
        var description = _fixture.Create<LinkDescription>();

        var newTags = new[] { LinkTag.New("Tag1"), LinkTag.New("Tag2"), LinkTag.New("Tag4"), LinkTag.New("Tag5"), LinkTag.New("Tag6") };

        // Act
        _testClass.Edit(title, description, newTags, UpdatedByUserIdValueObject);

        // Assert
        Assert.Equal(previousTitle, _testClass.Title);
        Assert.Equal(previousDescription, _testClass.Description);
        Assert.True(_testClass.IsDeleted);
        Assert.Equal(DeletedByUserIdValueObject.Value.ToString(), _testClass.DeletedById.Value.ToString());
        Assert.Equal(dateUpdatedDeleted, _testClass.DateDeleted);

        Assert.Equal(DeletedByUserIdValueObject.Value.ToString(), _testClass.UpdatedById.Value.ToString());
        Assert.Equal(dateUpdatedDeleted, _testClass.DateUpdated);

        Assert.Equal(previousTags.Count(), _testClass.TagsCollection.Count());
    }

    // Tests when the Title ValueObject is null
    [Fact]
    public void CannotCall_Edit_When_Title_IsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _testClass.Edit(
                default,
                _fixture.Create<LinkDescription>(),
                _fixture.Create<LinkTag[]>(),
                UpdatedByUserIdValueObject
                )
            );
    }


    [Fact]
    public void CannotCall_Edit_WithNull_UpdatedByUserId()
    {
        Assert.Throws<ArgumentNullException>(() => _testClass.Edit(_fixture.Create<LinkTitle>(), _fixture.Create<LinkDescription>(), _fixture.Create<LinkTag[]>(), default));
    }

    [Fact]
    public void CanCall_SetLikesCount()
    {
        var previousCount = _testClass.LikesCount;
        // Arrange
        var newCount = _fixture.Create<int>();

        // Act
        _testClass.SetLikesCount(newCount, UpdatedByUserIdValueObject);

        // Assert
        Assert.Equal(newCount, _testClass.LikesCount);
    }

    [Fact]
    public void CanNotCall_SetLikesCount_WithNegativeValue()
    {
        // Arrange
        Assert.Throws<ArgumentException>(() => _testClass.SetLikesCount(-1, UpdatedByUserIdValueObject));
    }

    [Fact]
    public void CannotCall_SetLikesCount_WithNull_UpdatedByUserId()
    {
        Assert.Throws<ArgumentNullException>(() => _testClass.SetLikesCount(_fixture.Create<int>(), default));
    }

    [Fact]
    public void CanCall_SetSavesCount()
    {
        var previousCount = _testClass.SavesCount;
        // Arrange
        var newCount = _fixture.Create<int>();

        // Act
        _testClass.SetSavesCount(newCount, UpdatedByUserIdValueObject);

        // Assert
        Assert.Equal(newCount, _testClass.SavesCount);
    }

    [Fact]
    public void CanNotCall_SetSavesCount_WithNegativeValue()
    {
        // Arrange
        Assert.Throws<ArgumentException>(() => _testClass.SetSavesCount(-1, UpdatedByUserIdValueObject));
    }

    [Fact]
    public void CannotCall_SetSavesCount_WithNull_UpdatedByUserId()
    {
        Assert.Throws<ArgumentNullException>(() => _testClass.SetSavesCount(_fixture.Create<int>(), default));
    }

    [Fact]
    public void CanCall_SetActiveState()
    {
        var previousState = _testClass.IsActive;

        // Arrange
        var isActive = !previousState;

        // Act
        _testClass.SetActiveState(isActive, UpdatedByUserIdValueObject);

        // Assert
        Assert.Equal(isActive, _testClass.IsActive);
    }

    [Fact]
    public void CannotCall_SetActiveState_WithNull_UpdatedByUserId()
    {
        Assert.Throws<ArgumentNullException>(() => _testClass.SetActiveState(_fixture.Create<bool>(), default));
    }

    [Fact]
    public void CanCall_SetFlaggedState()
    {
        var previousState = _testClass.IsFlagged;

        // Arrange
        var IsFlagged = !previousState;

        // Act
        _testClass.SetFlaggedState(IsFlagged, UpdatedByUserIdValueObject);

        // Assert
        Assert.Equal(IsFlagged, _testClass.IsFlagged);
    }

    [Fact]
    public void CannotCall_SetFlaggedState_WithNull_UpdatedByUserId()
    {
        Assert.Throws<ArgumentNullException>(() => _testClass.SetFlaggedState(_fixture.Create<bool>(), default));
    }

    [Fact]
    public void CanCall_UpdateTags_With_ArrayOfString()
    {
        // Arrange
        string[] newTags = new[] { "Tag1", "Tag3", "Tag5", "Tag7" };

        // Act
        _testClass.UpdateTags(newTags);

        // Assert
        Assert.NotNull(_testClass.TagsCollection);
        Assert.Equal(newTags.Length, _testClass.TagsCollection.Count());

        foreach (var newTag in newTags.Select(t => t.ToLower()).Distinct().ToArray())
        {
            Assert.NotNull(_testClass.TagsCollection.Select(t => t.Name.Equals(newTag)));
        }
    }

    /// <summary>
    /// If duplicates are passed in, they should be filtered out
    /// </summary>
    [Fact]
    public void CanCall_UpdateTags_With_Duplicate_ArrayOfString()
    {
        // Arrange
        string[] newTags = new[] { "Tag1", "Tag3", "Tag3", "Tag5", "Tag7", "tag7" };

        // Act
        _testClass.UpdateTags(newTags);

        // Assert
        Assert.NotNull(_testClass.TagsCollection);
        Assert.Equal(newTags.Select(t => t.ToLower()).Distinct().ToArray().Length, _testClass.TagsCollection.Count());

        foreach (var newTag in newTags.Select(t => t.ToLower()).Distinct().ToArray())
        {
            Assert.NotNull(_testClass.TagsCollection.Select(t => t.Name.Equals(newTag)));
        }
    }

    [Fact]
    public void CannotCall_UpdateTagsWithArrayOfString_WithNull_NewTags()
    {
        Assert.Throws<ArgumentNullException>(() => _testClass.UpdateTags(default(string[])));
    }

    [Fact]
    public void CanCall_UpdateTags_With_ArrayOfLinkTag()
    {
        // Arrange
        var newTags = new[] { LinkTag.New("Tag1"), LinkTag.New("Tag3"), LinkTag.New("Tag5"), LinkTag.New("Tag7") };

        // Act
        _testClass.UpdateTags(newTags);

        // Assert
        Assert.NotNull(_testClass.TagsCollection);
        Assert.Equal(newTags.Length, _testClass.TagsCollection.Count());

        foreach (var newTag in newTags.Select(t => t.Name.ToLower()).Distinct().ToArray())
        {
            Assert.NotNull(_testClass.TagsCollection.Select(t => t.Name.Equals(newTag)));
        }
    }

    /// <summary>
    /// If duplicates are passed in, they should be filtered out
    /// </summary>
    [Fact]
    public void CanCall_UpdateTags_With_Duplicate_ArrayOfLinkTag()
    {
        // Arrange
        var newTags = new[] { LinkTag.New("Tag1"), LinkTag.New("Tag3"), LinkTag.New("Tag5"), LinkTag.New("Tag5"), LinkTag.New("Tag7"), LinkTag.New("Tag7") };

        // Act
        _testClass.UpdateTags(newTags);

        // Assert
        Assert.NotNull(_testClass.TagsCollection);
        Assert.Equal(newTags.Select(t => t.Name.ToLower()).Distinct().ToArray().Length, _testClass.TagsCollection.Count());

        foreach (var newTag in newTags.Select(t => t.Name.ToLower()).Distinct().ToArray())
        {
            Assert.NotNull(_testClass.TagsCollection.Select(t => t.Name.Equals(newTag)));
        }
    }

    [Fact]
    public void CanGet_Id()
    {
        // Assert
        var result = Assert.IsType<LinkId>(_testClass.Id);

        Assert.NotNull(result);
        Assert.True(!result.Value.Equals(ObjectId.Empty));
    }

    [Fact]
    public void CanGet_Title()
    {
        // Assert
        var result = Assert.IsType<LinkTitle>(_testClass.Title);

        Assert.NotNull(result);
        Assert.Equal("Some title", result.Value);
    }

    [Fact]
    public void CanGet_Description()
    {
        // Assert
        var result = Assert.IsType<LinkDescription>(_testClass.Description);

        Assert.NotNull(result);
        Assert.Equal("This is some not so random description", result.Value);
    }

    [Fact]
    public void CanGet_Domain()
    {
        // Assert
        var result = Assert.IsType<LinkDomain>(_testClass.Domain);

        Assert.NotNull(result);
        Assert.Equal("delisc.io", result.Value);
    }

    [Fact]
    public void CanGet_ImageUrl()
    {
        // Assert
        var result = Assert.IsType<LinkImageUrl>(_testClass.ImageUrl);

        Assert.NotNull(result);
    }

    [Fact]
    public void CanGet_Keywords()
    {
        // Assert
        //var result = Assert.IsType<IReadOnlyCollection<string>>(_testClass.Keywords);
        var result = _testClass.Keywords;

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void CanGet_IsActive()
    {
        // Assert
        var result = Assert.IsType<bool>(_testClass.IsActive);

        Assert.True(result && !_testClass.IsDeleted);
    }

    [Fact]
    public void CanGet_IsFlagged()
    {
        // Assert
        var result = Assert.IsType<bool>(_testClass.IsFlagged);

        Assert.False(result);
    }

    [Fact]
    public void CanGet_SavesCount()
    {
        // Assert
        var result = Assert.IsType<int>(_testClass.SavesCount);

        Assert.True(result > 0);
    }

    [Fact]
    public void CanGet_LikesCount()
    {
        // Assert
        var result = Assert.IsType<int>(_testClass.LikesCount);

        Assert.True(result > 0);
    }

    [Fact]
    public void CanGet_TagsCollection()
    {
        // Assert
        var result = Assert.IsType<LinkTagCollection>(_testClass.TagsCollection);

        Assert.NotNull(result);
        Assert.True(result.Any());
    }

    [Fact]
    public void CanGet_Url()
    {
        // Assert
        var result = Assert.IsType<LinkUrl>(_testClass.Url);

        Assert.NotNull(result);
        Assert.Equal("https://www.delisc.io/some/url", result.Value);
    }

    [Fact]
    public void CanGet_CreatedById()
    {
        // Assert
        var actual = Assert.IsType<CreatedById>(_testClass.CreatedById);

        Assert.NotNull(actual);
        Assert.True(!actual.Value.Equals(ObjectId.Empty));
    }

    [Fact]
    public void CanGet_DateCreated()
    {
        // Assert
        var result = Assert.IsType<DateTime>(_testClass.DateCreated);

        Assert.True(result > DateTime.MinValue);
    }

    [Fact]
    public void CanGet_UpdatedById()
    {
        // Assert
        var result = Assert.IsType<UpdatedById>(_testClass.UpdatedById);

        Assert.NotNull(result);

        if (result.Value.Equals(ObjectId.Empty))
        {
            Assert.Null(_testClass.DateUpdated);
        }
        else
        {
            Assert.NotNull(_testClass.DateUpdated);
        }
    }

    [Fact]
    public void CanGet_DateUpdated()
    {
        // Assert
        var result = _testClass.DateUpdated;

        if (result is null)
        {
            // If DateUpdated is null, then no changes could have been made
            Assert.True(_testClass.UpdatedById.Value.Equals(ObjectId.Empty));
            Assert.False(_testClass.IsDeleted);
            Assert.True(_testClass.DeletedById.Value.Equals(ObjectId.Empty));
            Assert.False(_testClass.IsFlagged);
        }
        else
        {
            Assert.False(_testClass.UpdatedById.Value.Equals(ObjectId.Empty));
        }
    }

    [Fact]
    public void CanGet_IsDeleted()
    {
        // Assert
        var result = Assert.IsType<bool>(_testClass.IsDeleted);

        if (result)
        {
            Assert.True(!_testClass.DeletedById.Value.Equals(ObjectId.Empty));
            Assert.NotNull(_testClass.DateDeleted);
        }
        else
        {
            Assert.True(_testClass.DeletedById.Value.Equals(ObjectId.Empty));
            Assert.Null(_testClass.DateDeleted);
        }
    }

    [Fact]
    public void CanGet_DeletedById()
    {
        // Assert
        var result = Assert.IsType<DeletedById>(_testClass.DeletedById);

        Assert.NotNull(result);

        if (result.Value.Equals(ObjectId.Empty))
        {
            Assert.False(_testClass.IsDeleted);
            Assert.Null(_testClass.DateDeleted);
        }
        else
        {
            Assert.True(_testClass.IsDeleted);
            Assert.NotNull(_testClass.DateDeleted);
        }
    }

    [Fact]
    public void CanGet_DateDeleted()
    {
        // Assert
        var result = _testClass.DateDeleted;

        if (result is null)
        {
            Assert.False(_testClass.IsDeleted);
            Assert.True(_testClass.DeletedById.Value.Equals(ObjectId.Empty));
        }
        else
        {
            Assert.True(_testClass.IsDeleted);
            Assert.False(_testClass.DeletedById.Value.Equals(ObjectId.Empty));
        }
    }

    private void AssertLink(Link expected, Link actual)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Title.Value, actual.Title.Value);
        Assert.Equal(expected.Description.Value, actual.Description.Value);
        Assert.Equal(expected.Domain.Value, actual.Domain.Value);
        Assert.Equal(expected.ImageUrl.Value, actual.ImageUrl.Value);
        Assert.Equal(expected.Keywords, actual.Keywords);
        Assert.Equal(expected.IsActive, actual.IsActive);
        Assert.Equal(expected.IsFlagged, actual.IsFlagged);
        Assert.Equal(expected.SavesCount, actual.SavesCount);
        Assert.Equal(expected.LikesCount, actual.LikesCount);
        Assert.Equal(expected.Url.Value, actual.Url.Value);
        Assert.Equal(expected.IsDeleted, actual.IsDeleted);
        Assert.Equal(expected.DateDeleted, actual.DateDeleted);
        Assert.Equal(expected.DeletedById.Value, actual.DeletedById.Value);
        Assert.Equal(expected.CreatedById.Value, actual.CreatedById.Value);
        Assert.Equal(expected.DateCreated, actual.DateCreated);
        Assert.Equal(expected.UpdatedById.Value, actual.UpdatedById.Value);
        Assert.Equal(expected.DateUpdated, actual.DateUpdated);
        Assert.Equal(expected.TagsCollection.Count(), actual.TagsCollection.Count());
    }

    private LinkEntity GetInitEntity()
    {
        var entity = new LinkEntity()
        {
            Id = ObjectId.GenerateNewId(),
            Title = "Some title",
            Url = "https://www.delisc.io/some/url",
            Domain = "delisc.io",
            Description = "This is some not so random description",
            ImageUrl = "https://www.delisc.io/some/image/url.png",

            Keywords = new[] { "keyword1", "keyword2", "keyword3" },

            LikesCount = 12345,
            SavesCount = 54321,

            IsActive = true,
            IsFlagged = false,
            IsDeleted = false,

            CreatedById = ObjectId.GenerateNewId(),
            DateCreated = _fixture.Create<DateTime>(),
        };


        //entity.Id = ObjectId.GenerateNewId();

        //entity.Description = "Some description";
        //entity.Url = "https://www.delisc.io/some/url";

        //entity.IsActive = true;
        //entity.IsFlagged = false;
        //entity.IsDeleted = false;

        //entity.CreatedById = ObjectId.GenerateNewId();
        // entity.CreatedById = ObjectId.GenerateNewId();

        //entity.UpdatedById = ObjectId.GenerateNewId();
        //entity.DateUpdated = DateTimeOffset.UtcNow.AddDays(-10);

        //entity.DeletedById = ObjectId.Empty;
        //entity.DateDeleted = null;

        entity.Tags = new[]
        {
            new LinkTagEntity("tag1",10, 10/40m),
            new LinkTagEntity("tag2",20, 10/40m),
            new LinkTagEntity("tag3",5, 10/40m),
            new LinkTagEntity("tag4",3, 10/40m),
            new LinkTagEntity("tag5",2, 10/40m),
        };

        return entity;
    }
}