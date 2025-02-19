using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

public class LaunchWithDragTests
{
    [UnityTest]
    public IEnumerator ObjectBeforeAfterCollision_AnglesValid()
    {
        // Arrange
        // Create an object with the same settings as player ball
        var ball = new GameObject("Ball");
        var ballRb = ball.AddComponent<Rigidbody2D>();
        var ballCollider = ball.AddComponent<CircleCollider2D>();
        ballRb.gravityScale = 0;
        ballRb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Create a wall
        var wall = new GameObject("Wall");
        wall.AddComponent<BoxCollider2D>();
        wall.transform.position = new Vector2(2, 0);

        // Set up physics material matching the existing Ball material
        var ballMaterial = new PhysicsMaterial2D();
        ballMaterial.bounciness = 0.7f;
        ballMaterial.friction = 0;
        ballCollider.sharedMaterial = ballMaterial;

        // Initailize values used to calculate velocity
        var forceAmount = 8.5f;
        var startX = 5f;
        var endX = 0f;
        var startY = 5f;
        var endY = 0f;
        ballRb.linearVelocity = new Vector2((startX - endX) * forceAmount, (startY - endY) * forceAmount);

        // Record the angle of the ball's velocity before the collision
        float preCollisionAngle = Mathf.Atan2(ballRb.linearVelocity.y, ballRb.linearVelocity.x) * Mathf.Rad2Deg;

        // Act
        // Wait for the collision
        yield return new WaitForSeconds(0.5f);

        // Assert
        // Record angle of the ball's velocity after the collision
        float postCollisionAngle = Mathf.Atan2(ballRb.linearVelocity.y, ballRb.linearVelocity.x) * Mathf.Rad2Deg;

        // Expected: Angles are equal when one has its sign flipped.
        Assert.AreEqual(preCollisionAngle, -postCollisionAngle, 0.1f); // Allow small margin of error
    }
}
