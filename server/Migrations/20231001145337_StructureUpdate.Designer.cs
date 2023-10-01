﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(GalleriaHubDBContext))]
    [Migration("20231001145337_StructureUpdate")]
    partial class StructureUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("Models.ArtistVerification", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VerifierID")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserID", "VerifierID");

                    b.HasIndex("VerifierID");

                    b.ToTable("ArtistVerifications");
                });

            modelBuilder.Entity("Models.Discount", b =>
                {
                    b.Property<int>("DiscountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DiscountCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("DiscountPercentage")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<bool>("DiscountUsed")
                        .HasColumnType("INTEGER");

                    b.HasKey("DiscountID");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("Models.File", b =>
                {
                    b.Property<int>("FileID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileStorageReference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("FileID");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Models.Gallery", b =>
                {
                    b.Property<int>("GalleryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("GalleryID");

                    b.ToTable("Galleries");
                });

            modelBuilder.Entity("Models.GalleryFile", b =>
                {
                    b.Property<int>("GalleryID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FileID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Public")
                        .HasColumnType("INTEGER");

                    b.HasKey("GalleryID", "FileID");

                    b.HasIndex("FileID");

                    b.ToTable("GalleryFiles");
                });

            modelBuilder.Entity("Models.List", b =>
                {
                    b.Property<int>("ListID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("TEXT");

                    b.HasKey("ListID");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("Models.ListItem", b =>
                {
                    b.Property<int>("ListID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ListID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("ListItems");
                });

            modelBuilder.Entity("Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DiscountID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("User")
                        .HasColumnType("INTEGER");

                    b.HasKey("OrderID");

                    b.HasIndex("DiscountID");

                    b.HasIndex("User");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Models.OrderItem", b =>
                {
                    b.Property<int>("OrderID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductID")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("OrderID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FileID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(6, 2)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Public")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductID");

                    b.HasIndex("FileID");

                    b.HasIndex("UserID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Models.ProductFile", b =>
                {
                    b.Property<int>("ProductID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FileID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Public")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductID", "FileID");

                    b.HasIndex("FileID");

                    b.ToTable("ProductFiles");
                });

            modelBuilder.Entity("Models.ProductVerification", b =>
                {
                    b.Property<int>("ProductID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VerifierID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductID", "VerifierID");

                    b.HasIndex("VerifierID");

                    b.ToTable("ProductVerifications");
                });

            modelBuilder.Entity("Models.Review", b =>
                {
                    b.Property<int>("ReviewID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReviewContent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ReviewHeader")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReviewID");

                    b.HasIndex("ProductID");

                    b.HasIndex("UserID");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CoverPictureFileID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("DescriptionFileID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ListID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProfilePictureFileID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Public")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Surname")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("ListID");

                    b.HasIndex("PhoneNumber")
                        .IsUnique();

                    b.HasIndex("ProfilePictureFileID");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Models.UserList", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ListID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FileID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ListName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserID", "ListID");

                    b.HasIndex("FileID");

                    b.HasIndex("ListID");

                    b.ToTable("UserLists");
                });

            modelBuilder.Entity("Models.Verifier", b =>
                {
                    b.Property<int>("VerifierID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GalleryID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("VerifierID");

                    b.HasAlternateKey("UserID");

                    b.HasIndex("GalleryID");

                    b.ToTable("Verifiers");
                });

            modelBuilder.Entity("Models.ArtistVerification", b =>
                {
                    b.HasOne("Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Verifier", "Verifier")
                        .WithMany()
                        .HasForeignKey("VerifierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Verifier");
                });

            modelBuilder.Entity("Models.File", b =>
                {
                    b.HasOne("Models.User", null)
                        .WithOne("CoverPicture")
                        .HasForeignKey("Models.File", "FileID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.GalleryFile", b =>
                {
                    b.HasOne("Models.File", "File")
                        .WithMany()
                        .HasForeignKey("FileID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Gallery", "Gallery")
                        .WithMany()
                        .HasForeignKey("GalleryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("Gallery");
                });

            modelBuilder.Entity("Models.ListItem", b =>
                {
                    b.HasOne("Models.List", "List")
                        .WithMany()
                        .HasForeignKey("ListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("List");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Models.Order", b =>
                {
                    b.HasOne("Models.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountID");

                    b.HasOne("Models.User", "Customer")
                        .WithMany()
                        .HasForeignKey("User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Discount");
                });

            modelBuilder.Entity("Models.OrderItem", b =>
                {
                    b.HasOne("Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Models.Product", b =>
                {
                    b.HasOne("Models.File", "File")
                        .WithMany()
                        .HasForeignKey("FileID");

                    b.HasOne("Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.ProductFile", b =>
                {
                    b.HasOne("Models.File", "File")
                        .WithMany()
                        .HasForeignKey("FileID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Models.ProductVerification", b =>
                {
                    b.HasOne("Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Verifier", "Verifier")
                        .WithMany()
                        .HasForeignKey("VerifierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Verifier");
                });

            modelBuilder.Entity("Models.Review", b =>
                {
                    b.HasOne("Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.User", b =>
                {
                    b.HasOne("Models.List", "WishList")
                        .WithMany()
                        .HasForeignKey("ListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.File", "ProfilePicture")
                        .WithMany()
                        .HasForeignKey("ProfilePictureFileID");

                    b.Navigation("ProfilePicture");

                    b.Navigation("WishList");
                });

            modelBuilder.Entity("Models.UserList", b =>
                {
                    b.HasOne("Models.File", "File")
                        .WithMany()
                        .HasForeignKey("FileID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.List", "List")
                        .WithMany()
                        .HasForeignKey("ListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("List");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.Verifier", b =>
                {
                    b.HasOne("Models.Gallery", "Gallery")
                        .WithMany()
                        .HasForeignKey("GalleryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.User", "User")
                        .WithOne()
                        .HasForeignKey("Models.Verifier", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gallery");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Models.User", b =>
                {
                    b.Navigation("CoverPicture");
                });
#pragma warning restore 612, 618
        }
    }
}
