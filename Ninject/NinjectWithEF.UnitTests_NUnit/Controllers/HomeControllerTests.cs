using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using NinjectWithEF.Domain.Abstract;
using NinjectWithEF.Domain.Concrete;
using NinjectWithEF.Domain.Models;
using NinjectWithEF.WebUI.Controllers;
using NUnit.Framework;
using Moq;

//using NinjectWithEF.Domain.Abstract.Fakes;

namespace NinjectWithEF.UnitTests_NUnit.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<ISiteRepository> mockRepository;


        // this runs once when the class is instantiated before any tests are run.
        //[TestFixtureSetUp]
        //public void Init(TestContext context)
        //{

        //}

        [SetUp]
        public void RunBeforeAllTestsInClassAssembly()
        {
            // run before the test to allocate and configure resources
            // needed. This method will run BEFORE EACH test in the class.
            // add code that will run before any code in the entire test assembly is executed

            mockRepository = new Mock<ISiteRepository>();
        }

        #region Index

        public void IndexAction_ReturnsPostsList_ToListView()
        {
            // Use Assert.Inconclusive() to indicate that the test is still a work in progress. 
            // This will skip the test in test explorer
            
            // arrange
            // Create a mock instance of the ISiteRepository
            // that automatically creates an implementation of ISiteRepository and 
            // stores it in its Object property
            //Mock<ISiteRepository> mockRepository = new Mock<ISiteRepository>();

            // Since the index action method calls AllPosts(), we MUST configure AllPosts() 
            // in the mock object to return an empty list of Posts using the mock's SetUp method
            // as we are testing for an empty list
            // The Setup method takes a lambda expression as a parameter.
            // The Setup method allows us to supply an implementation for a particular method or property.
            mockRepository.Setup(a => a.AllPosts())
                          .Returns(new List<Post>()
                            {
                                new Post() { Title = "Some title 1", Content = "Some content"},
                                new Post() { Title = "Some title 2", Content = "Some content"},
                                new Post() { Title = "Some title 3", Content = "Some content"},
                                new Post() { Title = "Some title 4", Content = "Some content"}
                            });

            // create home controller instance and pass the mock object as a
            // parameter to the HomeController constructor
            // The ISiteRepository instance can be accessed through the Mock object’s Object property
            var homeController = new HomeController(mockRepository.Object);

            // act
            // get the result of index action method and save the results as a ViewResult
            var result = homeController.Index() as ViewResult;

            // asert
            //
            var model = (List<Post>)result.ViewData.Model;

            // verify that the model passed to the ViewData contains a collection of Posts objects
            CollectionAssert.AllItemsAreInstancesOfType(model, typeof(Post));

        }

        [Test]
        public void IndexAction_ModelIsTypeOfPostList()
        {
            //arrange
            //Mock<ISiteRepository> repositoryMock = new Mock<ISiteRepository>();

            mockRepository.Setup(a => a.AllPosts()).Returns(new List<Post>() { });

            // act
            var homeController = new HomeController(mockRepository.Object);

            ViewResult result = homeController.Index() as ViewResult;
            
            // assert
            Assert.IsInstanceOf(typeof(List<Post>), result.Model);
        }

        [Test]
        public void IndexAction_ReturnsEmptyPostsList_ToListView()
        {
            // arrange
            string expectedViewBagMessage = "No Posts Found";

            // arrange
            // Create a mock instance of the ISiteRepository
            // that automatically creates an implementation of ISiteRepository and 
            // stores it in its Object property
            //Mock<ISiteRepository> mockRepository = new Mock<ISiteRepository>();

            // Since the index action method calls AllPosts(), we MUST configure AllPosts() 
            // in the mock object to return an empty list of Posts using the mock's SetUp method
            // as we are testing for an empty list
            mockRepository.Setup(a => a.AllPosts()).Returns(new List<Post>(){});

            // create home controller instance and pass the mock object as a
            // parameter to the HomeController constructor
            // The ISiteRepository instance can be accessed through the Mock object’s Object property
            var homeController = new HomeController(mockRepository.Object);
            
            // act
            var result = homeController.Index() as ViewResult;

            // assert
            Assert.AreEqual(expectedViewBagMessage, result.ViewBag.Message);

            // Use Assert.Inconclusive() to indicate that the test is still a work in progress. 
            // This will skip the test in test explorer
            //Assert.Inconclusive();
        }

        /// <summary>
        /// This test validates that the redirect is going to the Index action after a post is saved
        /// </summary>
        /// 
        [Test]
        public void CreatePostAction_ModelStateValid_RedirectToIndexView()
        {
            // arrange
            string expected = "Index";
            //Mock<ISiteRepository> mockRespository = new Mock<ISiteRepository>();

            // NOTE:: You do not need to populate any properties of Post
            // because a test model validation does not occur when running your Action.
            Post post = new Post();

            var homeController = new HomeController(mockRepository.Object);

            // act
            // get the result of CreatePost(post) as a RedirectToRouteResult
            var result = homeController.CreatePost(post) as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }


        /// <summary>
        /// This test validates that the AddPost() method which returns void 
        /// in CreatePost action method is called
        /// </summary>
        /// 
        [Test]
        public void CreatePostAction_ModelStateValid_AddPostIsCalled()
        {
            // arrange
            //Mock<ISiteRepository> mockRespository = new Mock<ISiteRepository>();

            // NOTE:: You do not need to populate any properties of Post
            // because a test model validation does not occur when running your Action.
            Post post = new Post();

            var homeController = new HomeController(mockRepository.Object);

            // act
            // get the result of CreatePost(post) as a RedirectToRouteResult
            var result = homeController.CreatePost(post) as RedirectToRouteResult;

            // assert
            // The Mock.Verify() method is used for calling methods that have a void return type
            // to check if they are called.
            // The syntax to Verify is similar to Setup in that it takes
            // a lambda expression as a parameter.
            // The AddPost() has a void return type
            // the second parameter is used to check that the method was called only once
            mockRepository.Verify(a => a.AddPost(post), Times.Exactly(1));
        }

        /// <summary>
        /// This test simulate a model validation error,  
        /// so we should not call addPost() and return the current view.
        /// </summary>
        [Test]
        public void CreatePostAction_ModelStateNotValid_ReturnsCreateView()
        {
            // arrange
            string expected = "CreatePost";
            //Mock<ISiteRepository> repository = new Mock<ISiteRepository>();

            Post post = new Post();

            var homeController = new HomeController(mockRepository.Object);

            // add an error to the ModelState to make the model invalid
            homeController.ModelState.AddModelError("", "Title is required");
            
            // act
            var result = homeController.CreatePost(post) as ViewResult;
            
            // assert
            // The Mock.Verify() method is used for calling methods that have a void return type
            // to check if they are called.
            // The syntax to Verify is similar to Setup in that it takes
            // a lambda expression as a parameter.
            // The AddPost() has a void return type
            // the second parameter is used to check that the AddPost() method was not called
            // If the AddPost() method had not been called with the given parameter, 
            // Verify will throw a Moq.MockException
            mockRepository.Verify(a => a.AddPost(post), Times.Exactly(0));

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        #endregion

        #region About

        [Test]
        public void AboutAction_ReturnsAboutView()
        {
            // arrange
            string expected = "Your application description page.";

            var homeController = new HomeController(mockRepository.Object);

            // act
            var result = homeController.About() as ViewResult;

            // assert
            Assert.AreEqual(expected, result.ViewBag.Message);
        }

        #endregion


        [TearDown]
        public void RunsAfterEachTestClassasCompleted()
        {
            // runs AFTER EACH test in a test class.
            // can be used to clean up resources from the test
        }

        [TestFixtureTearDown]
        public static void RunAfterAllTestsInTestClassHaveCompleted()
        {
            // use to clean up any resourses left over from the test
        }
    }
}
